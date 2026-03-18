namespace GoVibe.API.Models.Reviews
{
    public class ReviewModel
    {
        public string Id { get; set; } = "";
        public string PlaceId { get; set; } = "";
        public int Rating { get; set; }
        public string Comment { get; set; } = "";
        public DateTime UpdatedAt { get; set; }

        public List<ReviewImageModel> Images { get; set; } = [];
    }

    public class AddReviewRequest
    {
        public string PlaceId { get; set; } = "";
        public int Rating { get; set; }
        public string Comment { get; set; } = "";

        public List<IFormFile> Images { get; set; } = [];
    }

    public class ReviewImageModel
    {
        public string Id { get; set; } = "";
        public Guid ReviewId { get; set; }
        public string ImageUrl { get; set; } = "";
    }
}