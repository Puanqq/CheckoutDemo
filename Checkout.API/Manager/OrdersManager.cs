using Checkout.API.DTOs;
using Checkout.API.Manager.Interfaces;
using Checkout.Entities.Models;
using Checkout.UnitOfWork.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.API.Manager
{
    public class OrdersManager : IOrdersManager
    {
        private readonly ICheckoutUnitOfWork context;       
        public OrdersManager(ICheckoutUnitOfWork context)
        {
            this.context = context;
        }
        public async Task<ActionResult<Order>> CreateNewOrder(List<CartDto> ListCart)
        {
            if (ListCart.Count == 0) return null;

            double total = 0;
            foreach(var item in ListCart)
            {                
                total += (double)item.Product.Price * item.Quantity;            
            }
            var order = new Order
            {
                Id = Guid.NewGuid(),
                Total = total,
                CreateAt = DateTime.UtcNow
            };

            context.Order.Add(order);
            try
            {
                await context.SaveChangeAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }

            foreach(var cart in ListCart)
            {
                if (await context.Product.GetAsync(cart.Product.Id) == null)
                {
                    await RemoveOrder(order.Id);                    
                    return null;
                }

                context.OrderDetail.Add(new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = cart.Product.Id,
                    Quantity = cart.Quantity,
                    CreatedAt = DateTime.UtcNow,
                });                
            }

            try
            {
                await context.SaveChangeAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                await RemoveOrder(order.Id);
                return null;
            }

            return order;
        }

        private async Task RemoveOrder(Guid id)
        {
            if(context.Order.Remove(id))
            {
                await context.SaveChangeAsync();
                var listOrderDetails = await context.OrderDetail.GetAllAsync();
                listOrderDetails = listOrderDetails.Where(x => x.OrderId == id);
                foreach (var item in listOrderDetails)
                {
                    if (!context.OrderDetail.Remove(item.Id))
                        throw new DbUpdateException();
                    await context.SaveChangeAsync();
                }
            }
        }
    }
}
