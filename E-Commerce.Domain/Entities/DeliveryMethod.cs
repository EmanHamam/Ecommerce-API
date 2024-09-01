﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class DeliveryMethod
    {
        public int DeliveryMethodId { get; set; }
        public string? ShortName { get; set; }
        public string? DeliveryTime { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
