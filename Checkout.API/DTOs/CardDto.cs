using Checkout.Entities.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.API.DTOs
{
    public class CardDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(1,99)]
        public int Quantity { get; set; }
    }
}
