﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Checkout.Entities.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreatedAt { get; set; }        
    }
}
