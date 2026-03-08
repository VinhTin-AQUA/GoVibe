namespace GoVibe.Domain.Entities
{
    public class ReviewImage : Entity
    {
        public Guid ReviewId { get; set; }
        public Review? Review { get; set; }
        
        public string ImageUrl { get; set; } = "";
    }
}