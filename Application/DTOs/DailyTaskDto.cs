using Application.Base;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DailyTaskDto : BaseEntityDto
    {
        public String Name { get; set; }

        public String Description { get; set; }

        public String Status { get; set; }

        public DateTime Date { get; set; }
        public Guid StaffId { get; set; }

        public IEnumerable<StaffDTO> Staffs { get; set; }

        public string StaffFullName { get; set; }

    }
}
