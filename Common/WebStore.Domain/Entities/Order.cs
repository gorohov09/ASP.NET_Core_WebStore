using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Domain.Entities
{
    public class Order : Entity
    {
        [Required]
        public User User { get; set; }

        [Required]
        [MaxLength(200)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        [NotMapped]
        public decimal TotalPrice => Items.Sum(item => item.TotalItemPrice);
    }
}
