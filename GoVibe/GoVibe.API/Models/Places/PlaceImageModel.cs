namespace GoVibe.API.Models.Places
{
    public class PlaceImageModel
    {
        public string Id { get; set; } = "";
        public string PlaceId { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
    }
    
    public class UploadRequest
    {
        public IFormFile? File { get; set; }
    }
}