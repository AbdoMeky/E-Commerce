using ECO.CORE.DTO.OrderItemDTO;
using ECO.CORE.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.DTO.OrderDTO
{
    public class AddOrderDTO
    {
        public List<AddOrderItemInOrderDTO>? OrderItems { get; set; }
        [Required]
        public string? UserId { get; set; }
    }
}
