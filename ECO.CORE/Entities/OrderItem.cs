using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Price at the time of purchase
        public int OrderId { get; set; }
        public Order Order { get; set; } // Navigation property
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
