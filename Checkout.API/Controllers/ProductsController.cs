using Checkout.Entities.Models;
using Checkout.UnitOfWork.Configurations;
using Checkout.UnitOfWork.IRepositories;
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
    public class ProductsController : ControllerBase
    {
        private readonly ICheckoutUnitOfWork context;
        public ProductsController(ICheckoutUnitOfWork context) 
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await context.Product.GetAllAsync();
            return products.ToList();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var course = await context.Product.GetAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await context.Product.UpdateAsync(product);

            try
            {
                await context.SaveChangeAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            context.Product.Add(product);            
            await context.SaveChangeAsync();                        

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await context.Product.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            context.Product.Remove(product.Id);
            await context.SaveChangeAsync();

            return NoContent();
        }

        private bool ProductExists(Guid id)
        {
            var product = context.Product.GetAsync(id);
            if (product == null)
                return false;
            return true;
        }
    }
}
