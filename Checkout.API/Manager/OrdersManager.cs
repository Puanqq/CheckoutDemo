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
        public async Task<ActionResult<Order>> CreateNewOrder(List<CardDto> ListCart)
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
                foreach (var item in ListCart)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product == null)
                    {
                        Log.Warning($"Product with Id {product.Id} is not found");
                        return new NotFoundResult();
                    }                    
                    total += (double)product.Price * item.Quantity;
                }
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    Total = total,
                    CreateAt = DateTime.UtcNow
                };

                _context.Orders.Add(order);
                try
                {
                    await _context.SaveChangesAsync();                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new Exception("Create blank order is fail!");
                }

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

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new Exception("Create order detail is fail!");
                }

                transaction.Commit();
                Log.Information("Create order is successful");
                return order;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                transaction.RollbackToSavepoint("BeforCreateOrder");
                return new BadRequestResult();
            }                                   
        }
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            /*var orders = await _context.Order.GetAllAsync();
            var orderDetails = await _context.OrderDetail.GetAllAsync();
            foreach (var order in orders)
            {
                order.Details = orderDetails.Where(o => o.OrderId == order.Id).ToList();
            }
            return orders.ToList();*/
            var list = await _context.Orders.
                Include(o => o.Details).ToListAsync();
            return list;
        }
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            /*var order = await _context.Order.GetAsync(id);
            if(order is null)
            {
                Log.Warning("Order is not exist");
                return new NotFoundResult();                
            }
            var orderDetails = await _context.OrderDetail.GetAllAsync();
            order.Details = orderDetails.Where(o => o.OrderId == order.Id).ToList();

            Log.Information("Get Order is successfull");
            return order;*/
            return _context.Orders.Where(b => b.Id == id)
                       .Include(b => b.Details)
                       .FirstOrDefault();            
        }
        public async Task<IActionResult> PutNewProductToOrder(Guid id, CardDto card)
        {
            var transaction = _context.Database.BeginTransaction();            
            try
            {                
                if (!OrderIsExist(id))
                {
                    Log.Warning("Order is not exist");
                    return new NotFoundResult();
                }

                var product = await _context.Products.FindAsync(card.ProductId);
                if (product == null)
                {
                    Log.Warning("Product is not exist!");
                    return new NotFoundResult();
                }

                transaction.CreateSavepoint("BeforUpdateOrder");

                var order = await _context.Orders.FindAsync(id);                
                var orderDetailExist = _context.OrderDetails.FirstOrDefault(o => o.OrderId == id && o.ProductId == card.ProductId);
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
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {                    
                    throw new Exception($"Update Order detail is fail: {ex.Message}");
                }

                order.Total += product.Price * card.Quantity;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex )
                {
                    throw new Exception($"Update Order is fail: {ex.Message}");
                }

                transaction.Commit();
                return new NoContentResult();

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                transaction.RollbackToSavepoint("BeforUpdateOrder");
                return new BadRequestResult();
            }                        
        }
        public async Task<IActionResult> DeleteProductInOrder(Guid id, Guid productId)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                if (!OrderIsExist(id))
                {
                    Log.Warning("This order isn't exist!");
                    return new NotFoundResult();
                }

                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    Log.Warning("This product isn't exist!");
                    return new NotFoundResult();
                }

                transaction.CreateSavepoint("BeforUpdateOrder");
                var order = await _context.Orders.FindAsync(id);                
                var orderDetailExist = _context.OrderDetails.FirstOrDefault(o => o.OrderId == id && o.ProductId == productId);
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
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {                    
                    throw new Exception($"Update Order detail is fail: {ex.Message}");
                }

                order.Total -= product.Price;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw new Exception($"Update Order is fail: {ex.Message}");
                }

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
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var transaction = _context.Database.BeginTransaction();

            try
            {
                if (!OrderIsExist(id))
                {
                    Log.Warning("This order isn't exist!");
                    return new NotFoundResult();
                }

                transaction.CreateSavepoint("BeforDelete");
                var order = await _context.Orders.FindAsync(id);                
                var orderDetailsMustRemove = _context.OrderDetails.Where(o => o.OrderId == id).ToList();
                foreach (var orderDetail in orderDetailsMustRemove)
                {
                    _context.OrderDetails.Remove(orderDetail);
                }
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {                    
                    throw new Exception($"Delete Detail order is fail: {ex.Message}");
                }

                _context.Orders.Remove(order);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {                    
                    throw new Exception($"Delete Order is fail : {ex.Message}");
                }

                transaction.Commit();
                Log.Information("Delete order is successful!");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                transaction.ReleaseSavepoint("BeforDelete");
                return new BadRequestResult();
            }
        }
        private bool OrderIsExist(Guid id)
        {
            return _context.Orders.Any(x => x.Id == id);
        }
    }
}
