﻿using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RoomType : BaseEntity
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Capacity { get; set; }
    }
}
