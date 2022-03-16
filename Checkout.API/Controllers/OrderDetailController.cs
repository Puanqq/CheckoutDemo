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
    public class OrderDetailController : ControllerBase
    {
        private readonly ICheckoutUnitOfWork _context;

        public OrderDetailController(ICheckoutUnitOfWork context)
        {
            _context = context;
        }
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetAllOrderDetail()
        {
            var orderDetail = await _context.OrderDetail.GetAllAsync();
            return orderDetail.ToList();
        }

        // GET: api/Products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetail(int id)
        {
            var orderDetail = await _context.OrderDetail.GetAsync(id);
            return orderDetail;
        }

        // PUT: api/Proucts/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutOrderDetail(int id, OrderDetail orderDetail)
        {
            if(id != orderDetail.Id)
            {
                return BadRequest();
            }

            await _context.OrderDetail.UpdateAsync(orderDetail);

            try
            {
                await _context.SaveChangeAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.OrderDetail.IsExist(id))
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
        public async Task<ActionResult<Product>> PostOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetail.Add(orderDetail);
            await _context.SaveChangeAsync();

            return CreatedAtAction("GetProduct", new {id = orderDetail.Id}, orderDetail);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var orderDetail = await _context.OrderDetail.GetAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.OrderDetail.Remove(orderDetail.Id);
            await _context.SaveChangeAsync();

            return NoContent();
        }
    }
}
