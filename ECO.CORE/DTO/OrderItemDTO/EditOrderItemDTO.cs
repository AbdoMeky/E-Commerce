﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.DTO.OrderItemDTO
{
    public class EditOrderItemDTO
    {
        [Required]
        public int Quantity { get; set; }
    }
}
