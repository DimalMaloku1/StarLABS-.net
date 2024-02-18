using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class AppUser : IdentityUser<Guid>
    {
		public string? Name { get; set; }
		public string? Surname { get; set; }
		public string? Role { get; set; }
	}
}
