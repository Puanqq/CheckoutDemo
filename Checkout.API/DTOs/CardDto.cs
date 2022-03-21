using Checkout.Entities.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.API.DTOs
{
    public class CardDto
    {
        public Guid ProductId { get; set; }
        [Range(1,99)]
        public int Quantity { get; set; }
    }
}
