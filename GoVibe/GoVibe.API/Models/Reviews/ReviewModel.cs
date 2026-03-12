namespace GoVibe.API.Models.Reviews
{
    public class ReviewModel
    {
        public Guid Id { get; set; }
        public Guid PlaceId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = "";
        public DateTime UpdatedAt { get; set; }

        public List<ReviewImageModel> Images { get; set; } = [];
    }

    public class ReviewImageModel
    {
        
    }
}