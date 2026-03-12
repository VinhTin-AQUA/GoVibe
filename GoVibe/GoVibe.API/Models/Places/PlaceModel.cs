using GoVibe.API.Models.Amenities;
using GoVibe.API.Models.Reviews;
using GoVibe.Domain.Entities;
using GoVibe.Domain.Enums;

namespace GoVibe.API.Models.Places
{
    public class PlaceModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public string Street { get; set; } = "";
        public string Ward { get; set; } = "";
        public string District { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";

        public string CategoryId { get; set; } = "";
        public Category? Category { get; set; }

        public string Phone { get; set; } = "";
        public string Website { get; set; } = "";
        public string OpeningHours { get; set; } = "";
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public EPlaceStatus Status { get; set; } = EPlaceStatus.None;
        public DateTime UpdatedAt { get; set; }
        
        public ICollection<PlaceImageModel> Images { get; set; } = [];
        public ICollection<ReviewModel> Reviews { get; set; } = [];
        public ICollection<AmenityModel> Amenities { get; set; } = [];
    }

    public class AddPlaceRequest
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public string Street { get; set; } = "";
        public string Ward { get; set; } = "";
        public string District { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";

        public string CategoryId { get; set; } = "";

        public string Phone { get; set; } = "";
        public string Website { get; set; } = "";
        public string OpeningHours { get; set; } = "";
        public EPlaceStatus Status { get; set; } = EPlaceStatus.None;
        
        public List<IFormFile> Images { get; set; } = [];
        public List<string> AmenityIds { get; set; } = [];
    }

    public class UpdatePlaceRequest
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public string Street { get; set; } = "";
        public string Ward { get; set; } = "";
        public string District { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public string CategoryId { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Website { get; set; } = "";
        public string OpeningHours { get; set; } = "";
        public EPlaceStatus Status { get; set; } = EPlaceStatus.None;
        
        public List<IFormFile> Images { get; set; } = []; // new Image
        public List<string> DeleteImages { get; set; } = []; // old images to remove
        public List<string> AmenityIds { get; set; } = [];
    }

    public class DeleteManyPlacesRequest
    {
        public List<string> Ids { get; set; } = [];
    }
}
