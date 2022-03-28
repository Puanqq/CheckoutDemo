using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace Checkout.Entities.Models
{
    public partial class Order
    {
        public Order()
        {
            Details = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public double? Total { get; set; }
        public DateTime? CreateAt { get; set; }                     
        public ICollection<OrderDetail> Details { get; set; }
    }
}
