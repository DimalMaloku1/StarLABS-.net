using Domain.Models;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PositionDTO
    {
            public string PositionName { get; set; }

            public string PositionDescription { get; set; }

            public Shift Shift { get; set; }      
    }
}