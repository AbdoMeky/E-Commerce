using ECO.CORE.DTO.OrderItemDTO;
using ECO.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.DTO.OrderDTO
{
    public class ShowOrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsRecieved { get; set; }
        public List<ShowOrderItemDTO>? OrderItems { get; set; }
        public string UserName { get; set; }
        public ShowOrderDTO()
        {
            
        }
        /*public ShowOrderDTO()
        {
            
        }*/
    }
}
