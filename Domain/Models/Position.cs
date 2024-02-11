using Domain.Base;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Position : BaseEntity
    {
        
        public string PositionName { get; set; }

        public string PositionDescription { get; set; }

        public Shift Shift { get; set; }
    }
}
