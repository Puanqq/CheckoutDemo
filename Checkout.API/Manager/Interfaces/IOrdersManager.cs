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
        Task<ActionResult<IEnumerable<OrderDto>>> GetOrders();
        Task<ActionResult<OrderDto>> GetOrder(int id);
        Task<ActionResult<OrderDto>> CreateNewOrder(List<CardDto> ListCart);        
        Task<IActionResult> PutNewProductToOrder(int id, CardDto card);
        Task<IActionResult> DeleteProductInOrder(int id, int ProductId);
        Task<IActionResult> DeleteOrder(int id);
    }
}
