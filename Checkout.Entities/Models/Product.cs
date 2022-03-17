using System;
using System.Collections.Generic;

#nullable disable

namespace Checkout.Entities.Models
{
    public partial class Product
    {
        public Guid Id { get; set; }
        public string PName { get; set; }
        public double? Price { get; set; }
        public DateTime? CreatedAt { get; set; }
    }  
}
