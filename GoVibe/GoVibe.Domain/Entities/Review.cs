namespace GoVibe.Domain.Entities
{
    public class Review : Entity
    {
        public Guid PlaceId { get; set; }
        public Place? Place { get; set; }
        
        public int Rating { get; set; }
        public string Comment { get; set; } = "";

        public ICollection<ReviewImage> Images { get; set; } = [];
    }
}