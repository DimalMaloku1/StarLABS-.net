using Domain.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Staff : BaseEntity
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
