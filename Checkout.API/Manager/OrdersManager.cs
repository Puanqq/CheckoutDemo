using AutoMapper;
using Checkout.API.DTOs;
using Checkout.API.Manager.Interfaces;
using Checkout.Entities.Models;
using Checkout.UnitOfWork.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.API.Manager
{
    public class OrdersManager : IOrdersManager
    {       
        private readonly CheckoutDemoContext _context;
        private readonly IMapper _mapper;        

        public OrdersManager(IMapper mapper, CheckoutDemoContext context)
        {            
            _mapper = mapper;
            _context = context;
        }
        public async Task<ActionResult<OrderDto>> CreateNewOrder(List<CardDto> ListCart)
        {
            var transaction = _context.Database.BeginTransaction();            
            try
            {
                transaction.CreateSavepoint("BeforCreateOrder");

                if (ListCart.Count == 0)
                {
                    Log.Warning("List cart is empty to create order");
                    return null;
                }

                double total = 0;
                var products = await _context.Products.ToListAsync();
                foreach (var item in ListCart)
                {
                    var product = products.Where(x => x.Id == item.ProductId).FirstOrDefault();
                    if (product == null)
                    {
                        Log.Warning($"Product with Id {product.Id} is not found");
                        return new NotFoundResult();
                    }                    
                    total += (double)product.Price * item.Quantity;
                }
                var order = new Order
                {                    
                    Total = total,
                    CreateAt = DateTime.UtcNow                    
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var cart in ListCart)
                {                    
                    _context.OrderDetails.Add(new OrderDetail
                    {                        
                        OrderId = order.Id,
                        ProductId = cart.ProductId,
                        Quantity = cart.Quantity,
                        CreatedAt = DateTime.UtcNow,
                    });
                }

                await _context.SaveChangesAsync();

                transaction.Commit();
                Log.Information("Create order is successful");
                return _mapper.Map<OrderDto>(order);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                transaction.RollbackToSavepoint("BeforCreateOrder");
                return new BadRequestResult();
            }                                   
        }
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Details)  
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    Total = (double)o.Total,
                    CreateAt = (DateTime)o.CreateAt,
                    DetailOder = (ICollection<OrderDetailDto>)o.Details.Select(x => new OrderDetailDto
                    {
                        OrderId = x.Id,
                        CreatedAt = x.CreatedAt,
                        Id = x.Id,
                        ProductId = x.ProductId,
                        Quantity = x.Quantity
                    })
                })
                .ToListAsync();

            /*var orders2 = from order in _context.Orders
                          join odetail in _context.OrderDetails on order.Id equals odetail.OrderId
                          select new OrderDto
                          {
                              Id = order.Id,
                              Total = (double)order.Total,
                              CreateAt = (DateTime)order.CreateAt,
                              DetailOder = (ICollection<OrderDetailDto>)order.Details.Select(x => new OrderDetailDto
                              {
                                  OrderId = x.Id,
                                  CreatedAt = x.CreatedAt,
                                  Id = x.Id,
                                  ProductId = x.ProductId,
                                  Quantity = x.Quantity
                              })
                          };*/

            return orders;
        }
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {            
            var order = await _context.Orders
                                    .Where(b => b.Id == id)
                                    .Include(b => b.Details)                                    
                                    .FirstOrDefaultAsync();
            return _mapper.Map<OrderDto>(order);            
        }
        public async Task<IActionResult> PutNewProductToOrder(int id, CardDto card)
        {
            var transaction = _context.Database.BeginTransaction();            
            try
            {                                
                var product = await _context.Products.FindAsync(card.ProductId);
                if (product == null)
                {
                    Log.Warning("Product is not exist!");
                    return new NotFoundResult();
                }

                transaction.CreateSavepoint("BeforUpdateOrder");

                var order = await _context.Orders.Where(b => b.Id == id).Include(b => b.Details).FirstOrDefaultAsync();

                if (order == null)
                {
                    Log.Warning("Order is not exist");
                    return new NotFoundResult();
                }

                var orderDetailExist = order.Details.FirstOrDefault(x => x.OrderId == card.ProductId);
                if (orderDetailExist is null)
                {
                    _context.OrderDetails.Add(new OrderDetail
                    {                        
                        ProductId = card.ProductId,
                        OrderId = id,
                        Quantity = card.Quantity,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                else
                {
                    orderDetailExist.Quantity += card.Quantity;
                }                
                order.Total += product.Price * card.Quantity;
                
                await _context.SaveChangesAsync();
                transaction.Commit();
                return new OkResult();

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                transaction.RollbackToSavepoint("BeforUpdateOrder");
                return new BadRequestResult();
            }                        
        }
        public async Task<IActionResult> DeleteProductInOrder(int id, int productId)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {                
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    Log.Warning("This product isn't exist!");
                    return new NotFoundResult();
                }

                transaction.CreateSavepoint("BeforUpdateOrder");
                var order = await _context.Orders.Where(b => b.Id == id).Include(b => b.Details).FirstOrDefaultAsync();

                if (order == null)
                {
                    Log.Warning("This order isn't exist!");
                    return new NotFoundResult();
                }

                var orderDetailExist = order.Details.FirstOrDefault(x => x.OrderId == productId);
                if (orderDetailExist is null)
                {
                    Log.Warning("In order isn't have this product to delete!");
                    return new NotFoundResult();
                }
                else
                {
                    if(orderDetailExist.Quantity == 1)
                    {
                        _context.OrderDetails.Remove(orderDetailExist);
                    }
                    else
                    {
                        orderDetailExist.Quantity -= 1;
                    }
                }

                await _context.SaveChangesAsync();

                transaction.Commit();
                return new NoContentResult();
            }
            catch(Exception ex)
            {
                Log.Warning(ex.Message);
                transaction.RollbackToSavepoint("BeforUpdateOrder");
                return new BadRequestResult();
            }
        }
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (!OrderIsExist(id))
            {
                Log.Warning("This order isn't exist!");
                return new NotFoundResult();
            }
            
            var order = await _context.Orders.Where(b => b.Id == id).Include(b => b.Details).FirstOrDefaultAsync();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            
            Log.Information("Delete order is successful!");
            return new NoContentResult();
        }
        private bool OrderIsExist(int id)
        {
            return _context.Orders.Any(x => x.Id == id);
        }
    }
}
