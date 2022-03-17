using Checkout.API.DTOs;
using Checkout.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkout.API.Manager.Interfaces
{
    public interface IOrdersManager
    {
        Task<ActionResult<Order>> CreateNewOrder(List<CartDto> ListCart);
    }
}
