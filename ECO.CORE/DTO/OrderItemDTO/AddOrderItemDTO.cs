﻿using ECO.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.DTO.OrderItemDTO
{
    public class AddOrderItemDTO
    {
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public int? ProductId { get; set; }
    }
}
