using GoVibe.Domain.Enums;

namespace GoVibe.Domain.Entities
{
    public class Place : Entity
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        
        public string Address { get; set; } = "";
        public string Country { get; set; } = "";

        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        
        public string Phone { get; set; } = "";
        public string Website { get; set; } = "";
        public string OpeningHours { get; set; } = "";
        
        public int TotalViews { get; set; }
        public double TotalRating { get; set; }
        public int TotalReviews { get; set; }
        public string Thumbnail { get; set; } = "";

        public EPlaceStatus Status { get; set; } = EPlaceStatus.None;
        public ICollection<PlaceImage> Images { get; set; } = [];
        public ICollection<Review> Reviews { get; set; } = [];
    }
}