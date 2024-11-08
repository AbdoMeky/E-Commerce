﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.DTO.PaymentDTO
{
    public class PaymentIntentCreateRequestDTO
    {
        public long Amount { get; set; }
        public string Currency { get; set; } = "usd";
    }
}
