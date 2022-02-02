﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Domain.Entities.Orders
{
    public class OrderItem
    {
        [Required]
        public Product Product { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public Order Order { get; set; }

        [NotMapped]
        public decimal TotalItemPrice => Price * Quantity;
    }
}
