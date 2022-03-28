using System;

namespace Checkout.API.DTOs
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
