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
    public class ProductsController : ControllerBase
    {
        private readonly ICheckoutUnitOfWork _context;

        public ProductsController(ICheckoutUnitOfWork context)
        {
            _context = context;
        }
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _context.Product.GetAllAsync();
            return products.ToList();
        }

        // GET: api/Products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.GetAsync(id);
            return product;
        }

        // PUT: api/Proucts/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, Product product)
        {
            if(id != product.Id)
            {
                return BadRequest();
            }

            await _context.Product.UpdateAsync(product);

            try
            {
                await _context.SaveChangeAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Product.IsExist(id))
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
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangeAsync();

            return CreatedAtAction("GetProduct", new {id = product.Id}, product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Product.GetAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Product.Remove(course.Id);
            await _context.SaveChangeAsync();

            return NoContent();
        }
    }
}
