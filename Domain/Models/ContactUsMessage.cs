using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;

namespace Domain.Models
{
    public class ContactUsMessage : BaseEntity
    {
        public Guid? UserId { get; set; }
        public AppUser? User { get; set; }
        public string MessageContent { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Replied { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}