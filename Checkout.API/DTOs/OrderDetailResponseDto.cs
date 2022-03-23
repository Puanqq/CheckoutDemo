using System;

namespace Checkout.API.DTOs
{
    public class OrderDetailResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderDetailResponseDto Order { get; set; }
    }
}
