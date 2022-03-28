using AutoMapper;
using Checkout.API.DTOs;
using Checkout.API.Filters;
using Checkout.API.Manager.Interfaces;
using Checkout.Entities.Models;
using Checkout.UnitOfWork.Configurations;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class OrdersController : ControllerBase
    {        
        private readonly IOrdersManager _ordersManager;
        private readonly IMapper mapper;
        public OrdersController(IOrdersManager ordersManager, IMapper mapper)
        {            
            _ordersManager = ordersManager;
            this.mapper = mapper;
        }

        //GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _ordersManager.GetOrders();            
        }

        // GET: api/Orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            return await _ordersManager.GetOrder(id);
        }

        // PUT: api/Orders/{id}/cards        
        [HttpPut]
        [Route("{id}/cards")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> PutOrderDetailInOrder(Guid id, [FromBody] CardDto card)
        {            
            return await _ordersManager.PutNewProductToOrder(id, card);
        }

        //DELETE: api/Orders/{Order id}/products/{product id}
        [HttpDelete]
        [Route("{id}/products/{ProductId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> DeleteOrderDetailInOrder(Guid id, Guid ProductId)
        {
            return await _ordersManager.DeleteProductInOrder(id, ProductId);
        }

        // POST: api/Orders/checkout        
        [HttpPost]
        [Route("checkout")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<Order>> PostOrder([FromBody] List<CardDto> ListCarts)
        {            
            return await _ordersManager.CreateNewOrder(ListCarts);            
        }

        // DELETE: api/Orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            return await _ordersManager.DeleteOrder(id);
        }       
    }
}
