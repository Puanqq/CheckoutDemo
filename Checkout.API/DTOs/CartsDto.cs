using Checkout.Entities.Models;

namespace Checkout.API.DTOs
{
    public class CartDto
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
