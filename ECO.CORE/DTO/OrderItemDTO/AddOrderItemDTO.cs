using ECO.CORE.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.DTO.OrderItemDTO
{
    public class AddOrderItemDTO
    {
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
