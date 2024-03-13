using Domain.Base;
namespace Domain.Models
{
    public class RoomType : BaseEntity
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Capacity { get; set; }
        public List<RoomTypePhoto> Photos { get; set; }
    }

}