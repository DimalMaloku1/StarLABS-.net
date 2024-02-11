using Domain.Base;

namespace Domain.Models
{
    public class Feedback : BaseEntity
    {
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
