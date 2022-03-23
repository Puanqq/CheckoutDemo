using System;
using System.Collections.Generic;

#nullable disable

namespace Checkout.Entities.Models
{
    public partial class Order
    {
        public Order()
        {
            Details = new HashSet<OrderDetail>();
        }

        public Guid Id { get; set; }
        public double? Total { get; set; }
        public DateTime? CreateAt { get; set; }            
        public ICollection<OrderDetail> Details { get; set; }
    }
}
