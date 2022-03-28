using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.API.DTOs
{
    public class ProductDto
    {
        [Required]
        public int Id { get; set; }
        public string ProductName { get; set; }
        [Range(1,999_999_999_999.9)]
        public double? ProductPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
    public class ProductOutputDto : ProductDto { }
}
