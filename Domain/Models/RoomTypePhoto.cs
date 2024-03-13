using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
        public class RoomTypePhoto : BaseEntity
    {
            public byte[] PhotoData { get; set; }
            public string ContentType { get; set; }
            public Guid RoomTypeId { get; set; }
            [JsonIgnore]
            public RoomType RoomType { get; set; }
     }
 }

