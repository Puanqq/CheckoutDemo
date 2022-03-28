using AutoMapper;
using Checkout.API.DTOs;
using Checkout.API.Manager.Interfaces;
using Checkout.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.API.Manager
{
    public class ProductManager : IProductManager
    {
        private readonly CheckoutDemoContext _context;
        private readonly IMapper _mapper;

        public ProductManager(CheckoutDemoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return new NotFoundResult();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Products.Select(x => new ProductDto
            {
                Id = id,
                ProductName = x.PName,
                ProductPrice = x.Price,
                CreatedAt = x.CreatedAt,
            }).FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return new NotFoundResult();
            }

            return product;
        }

        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _context.Products.Select(x => new ProductDto
            {
                Id = x.Id,
                ProductName = x.PName,
                ProductPrice = x.Price,
                CreatedAt = x.CreatedAt,
            }).ToListAsync();
            return products;
        }

        public async Task<ActionResult<ProductDto>> PostProduct(ProductDto productDto)
        {
            _context.Products.Add(_mapper.Map<Product>(productDto));
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<ActionResult> PutProduct(int id, ProductDto productDto)
        {
            if (id != productDto.Id)
            {
                return new BadRequestResult();
            }

            if (!ProductExists(id))
                return new NotFoundResult();

            var product = _mapper.Map<Product>(productDto);
            //_context.Entry(product).State = EntityState.Modified;
            //or
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
