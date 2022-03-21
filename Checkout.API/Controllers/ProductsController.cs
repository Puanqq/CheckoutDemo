using AutoMapper;
using Checkout.API.DTOs;
using Checkout.API.Filters;
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
        private readonly ICheckoutUnitOfWork _context;
        private readonly IMapper mapper;
        public ProductsController(ICheckoutUnitOfWork context, IMapper mapper) 
        {
            _context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Product.GetAllAsync();
            return products.ToList();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var course = await _context.Product.GetAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, ProductDto product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            Product productResult = mapper.Map<Product>(product);
            await _context.Product.UpdateAsync(productResult);

            try
            {
                await _context.SaveChangeAsync();
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<Product>> PostProduct(ProductDto product)
        {
            Product productResult = mapper.Map<Product>(product);
            _context.Product.Add(productResult);                            
            await _context.SaveChangeAsync();                        

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Product.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product.Id);
            await _context.SaveChangeAsync();

            return NoContent();
        }

        private bool ProductExists(Guid id)
        {
            var product = _context.Product.GetAsync(id);
            if (product == null)
                return false;
            return true;
        }
    }
}
