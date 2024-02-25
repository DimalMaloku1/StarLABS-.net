using Application.Base;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class StaffDTO : BaseEntityDto
    {
        public string Department { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }

        public Guid UserId { get; set; }
        public Guid PositionId { get; set; }
        public AppUser User { get; set; }
        public Position Position { get; set; }
    }
}
