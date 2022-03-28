using Checkout.API.DTOs;
using Checkout.API.Filters;
using Checkout.API.Manager.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkout.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrdersController : ControllerBase
    {        
        private readonly IOrdersManager _ordersManager;        
        public OrdersController(IOrdersManager ordersManager)
        {            
            _ordersManager = ordersManager;            
        }

        //GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            return await _ordersManager.GetOrders();            
        }

        // GET: api/Orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            return await _ordersManager.GetOrder(id);
        }

        // PUT: api/Orders/{id}/cards        
        [HttpPut]
        [Route("{id}/cards")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> PutOrderDetailInOrder(int id, [FromBody] CardDto card)
        {            
            return await _ordersManager.PutNewProductToOrder(id, card);
        }

        //DELETE: api/Orders/{Order id}/products/{product id}
        [HttpDelete]
        [Route("{id}/products/{ProductId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> DeleteOrderDetailInOrder(int id, int ProductId)
        {
            return await _ordersManager.DeleteProductInOrder(id, ProductId);
        }

        // POST: api/Orders/checkout        
        [HttpPost]
        [Route("checkout")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<OrderDto>> PostOrder([FromBody] List<CardDto> ListCarts)
        {            
            return await _ordersManager.CreateNewOrder(ListCarts);            
        }

        // DELETE: api/Orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            return await _ordersManager.DeleteOrder(id);
        }       
    }
}
