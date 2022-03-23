using System;
using System.Collections.Generic;

namespace Checkout.API.DTOs
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public double Total { get; set; }
        public DateTime CreateAt { get; set; }
        public ICollection<OrderDetailResponseDto>? Details { get; set; }
    }
}
