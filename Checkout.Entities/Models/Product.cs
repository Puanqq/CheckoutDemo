using System;
using System.Collections.Generic;

#nullable disable

namespace Checkout.Entities.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string PName { get; set; }
        public double? Price { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
