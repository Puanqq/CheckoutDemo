using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.API.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        [StringLength(50)]
        public string PName { get; set; }
        [Range(0, 9999.99)]
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
