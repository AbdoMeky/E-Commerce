using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsRecieved { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
