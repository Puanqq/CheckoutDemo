using System;
using System.Collections.Generic;

#nullable disable

namespace Checkout.Entities.Models
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? Amount { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual OrderDetail Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
