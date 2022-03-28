using System;
using System.Collections.Generic;

namespace Checkout.API.DTOs
{
    public class OrderDto
    {        
        public int Id { get; set; }
        public double Total { get; set; }
        public DateTime CreateAt { get; set; }
        public ICollection<OrderDetailDto> DetailOder { get; set; }
    }
}
