using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;

namespace Domain.Models
{
    public class Log : BaseEntity
    {
        public string? Action { get; set; } 
        public string? Entity { get; set; } 
        public string? UserName { get; set; } 
        public DateTime Timestamp { get; set; } 
    }

}
