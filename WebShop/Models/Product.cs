using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    internal class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public int? CategoryId { get; set; }

        // Navigation property for the category this product belongs to
        public virtual Category? Category { get; set; }
    }
}
