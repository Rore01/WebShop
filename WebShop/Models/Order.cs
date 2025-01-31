using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    internal class Order
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public bool Purchased { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
