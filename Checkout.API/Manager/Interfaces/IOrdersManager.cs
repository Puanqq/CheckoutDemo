using Checkout.API.DTOs;
using Checkout.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkout.API.Manager.Interfaces
{
    public interface IOrdersManager
    {
        Task<ActionResult<IEnumerable<Order>>> GetOrders();
        Task<ActionResult<Order>> GetOrder(Guid id);
        Task<ActionResult<Order>> CreateNewOrder(List<CardDto> ListCart);        
        Task<IActionResult> PutNewProductToOrder(Guid id, CardDto card);
        Task<IActionResult> DeleteProductInOrder(Guid id, Guid ProductId);
        Task<IActionResult> DeleteOrder(Guid id);
    }
}
