using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class DailyTask : BaseEntity
    {
        public String Name { get; set; }

        public String Description { get; set; }

        public String Status { get; set; }

        public DateTime Date { get; set; }
        public Guid StaffId { get; set; }
        [ForeignKey("StaffId")]
        public Staff Staff { get; set; }


    }
}