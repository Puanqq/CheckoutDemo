using Checkout.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkout.API.Manager.Interfaces
{
    public interface IProductManager
    {
        Task<ActionResult<IEnumerable<ProductDto>>> GetProducts();
        Task<ActionResult<ProductDto>> GetProduct(int id);
        Task<ActionResult<ProductDto>> PostProduct(ProductDto productDto);
        Task<ActionResult> DeleteProduct(int id);
        Task<ActionResult> PutProduct(int id, ProductDto productDto);
    }
}
