using Checkout.API.DTOs;
using Checkout.API.Manager.Interfaces;
using Checkout.Entities.Models;
using Checkout.UnitOfWork.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ICheckoutUnitOfWork context;
        private IOrdersManager ordersManager;
        public OrdersController(ICheckoutUnitOfWork context, IOrdersManager ordersManager)
        {
            this.context = context;
            this.ordersManager = ordersManager;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await context.Order.GetAllAsync();
            return orders.ToList();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await context.Order.GetAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(Guid id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            await context.Order.UpdateAsync(order);

            try
            {
                await context.SaveChangeAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("checkout/products")]
        public async Task<ActionResult<Order>> PostOrder([FromBody] List<CartDto> ListCarts)
        {
            return await ordersManager.CreateNewOrder(ListCarts);            
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await context.Order.GetAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            context.Order.Remove(order.Id);
            await context.SaveChangeAsync();

            return NoContent();
        }

        private bool OrderExists(Guid id)
        {
            var order = context.Order.GetAsync(id);
            if (order == null)
                return false;
            return true;
        }
    }
}
