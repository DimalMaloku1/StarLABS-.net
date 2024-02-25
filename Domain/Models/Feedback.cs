using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Feedback : BaseEntity
    {
        public string Comment { get; set; }
        public int Rating { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
    }
}
