using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Checkout.Entities.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string PName { get; set; }        
        public double? Price { get; set; }
        public DateTime? CreatedAt { get; set; }
    }  
}
