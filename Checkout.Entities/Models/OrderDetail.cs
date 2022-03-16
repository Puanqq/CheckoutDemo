using System;
using System.Collections.Generic;

#nullable disable

namespace Checkout.Entities.Models
{
    public partial class OrderDetail
    {
        public OrderDetail()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public double? Total { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
