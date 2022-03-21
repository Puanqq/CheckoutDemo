using System;
using System.Collections.Generic;

namespace Checkout.API.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public double Total { get; set; }
        public DateTime CreateAt { get; set; }
        public List<OrderDetailDto>? OrderDetails { get; set; }
    }
}
