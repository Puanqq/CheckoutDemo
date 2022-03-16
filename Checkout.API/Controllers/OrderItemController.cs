using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Checkout.API;
using Checkout.UnitOfWork.Configurations;
using Checkout.Entities.Models;

namespace Checkout.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly ICheckoutUnitOfWork _context;

        public OrderItemController(ICheckoutUnitOfWork context)
        {
            _context = context;
        }
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetAllOrderItem()
        {
            var orderItems = await _context.OrderItem.GetAllAsync();
            return orderItems.ToList();
        }

        // GET: api/Products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var orderItem = await _context.OrderItem.GetAsync(id);
            return orderItem;
        }

        // PUT: api/Proucts/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutOrderItem(int id, OrderItem orderItem)
        {
            if(id != orderItem.Id)
            {
                return BadRequest();
            }

            await _context.OrderItem.UpdateAsync(orderItem);

            try
            {
                await _context.SaveChangeAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.OrderItem.IsExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostOrderItem(OrderItem orderItem)
        {
            _context.OrderItem.Add(orderItem);
            await _context.SaveChangeAsync();

            return CreatedAtAction("GetProduct", new {id = orderItem.Id}, orderItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _context.OrderItem.GetAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.OrderItem.Remove(orderItem.Id);
            await _context.SaveChangeAsync();

            return NoContent();
        }
    }
}
