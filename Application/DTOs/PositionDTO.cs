using Domain.Models;
using Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Base;

namespace Application.DTOs
{
    public class PositionDTO : BaseEntityDto
    {
            public string PositionName { get; set; }

            public string PositionDescription { get; set; }

            public Shiftdto Shift { get; set; }      
    }
}