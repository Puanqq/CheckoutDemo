using Checkout.API.Enums;
using Checkout.Entities.Models;
using Checkout.UnitOfWork.Configurations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutUnitOfWork _context;
        public CheckoutsController(ICheckoutUnitOfWork context)
        {
            _context = context;
        }
        //create Order
        [HttpPut("{id}")]
        public async Task<ActionResult> AddItemToExistOrder(int id, int productId)
        {
            var product = await _context.Product.GetAsync(productId);
            if (product == null)
                return BadRequest("Product is not exist");            

            var listOrderItem = await _context.OrderItem.GetAllAsync();
            var existOrderItem = listOrderItem.FirstOrDefault(o => o.OrderId == id && o.ProductId == productId);
            if (existOrderItem != null)
            {
                existOrderItem.Amount += 1;
            }
            else
            {
                _context.OrderItem.Add(new OrderItem
                {
                    Id = await CreateId(OptionModel.OrderItem),
                    OrderId = id,
                    ProductId = productId,
                    Amount = 1,
                    CreatedAt = DateTime.UtcNow
                });
            }

            var orderDetail = await _context.OrderDetail.GetAsync(id);
            orderDetail.Total += product.Price;

            await _context.SaveChangeAsync();
            return NoContent();
        }

        // POST: api/Checkout
        [HttpPost("{id}")]
        public async Task<ActionResult> AddNewItemAndCreateOrder(int id)
        {                        
            var product = await _context.Product.GetAsync(id);

            if (product == null)
                return BadRequest("Product is not exist");

            var orderDetail = new OrderDetail
            {
                Id = await CreateId(OptionModel.OrderDetail),
                Total = product.Price,
                CreatedAt = DateTime.UtcNow
            };
            await _context.OrderDetail.CreateAsync(orderDetail);
            await _context.SaveChangeAsync();

            //var orderDatilAf = await _context.OrderDetail.GetAsync(orderDetail.Id);

            _context.OrderItem.Add(new OrderItem
            {
                Id = await CreateId(OptionModel.OrderItem),
                OrderId = orderDetail.Id,
                ProductId = product.Id,
                Amount = 1,
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangeAsync();

            return NoContent();
        }

        private async Task<int> CreateId(OptionModel option)
        {            
            switch (option)
            {
                case OptionModel.Product:
                    {
                        var lists = await _context.Product.GetAllAsync();
                        return lists.Count();
                    };
                case OptionModel.OrderItem:
                    {
                        var lists = await _context.OrderItem.GetAllAsync();
                        return lists.Count();
                    };
                case OptionModel.OrderDetail:
                    {
                        var lists = await _context.OrderDetail.GetAllAsync();
                        return lists.Count();
                    };
            }
            return await Task.FromResult(0);
        }
    }
}
