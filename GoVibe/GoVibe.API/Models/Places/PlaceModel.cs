using GoVibe.API.Models.Categories;
using GoVibe.API.Models.Reviews;
using GoVibe.Domain.Enums;

namespace GoVibe.API.Models.Places
{
    public class PlaceModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";

        public double AverageRating { get; set; }
        public int TotalRating { get; set; }
        
        public int TotalViews { get; set; }
        public int TotalReviews { get; set; }
        public string Thumbnail { get; set; } = "";
        
        public EPlaceStatus Status { get; set; } = EPlaceStatus.None;
        public DateTime UpdatedAt { get; set; }
        
        public CategoryModel? Category { get; set; }
    }

    public class PlaceDetailsModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Address { get; set; } = "";
        public string Country { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Website { get; set; } = "";
        public string OpeningHours { get; set; } = "";
        public int TotalViews { get; set; }
        public double TotalRating { get; set; }
        public int TotalReviews { get; set; }
        public DateTime UpdatedAt { get; set; }
        public EPlaceStatus Status { get; set; } = EPlaceStatus.None;

        public ICollection<PlaceImageModel> Images { get; set; } = [];
        public ICollection<ReviewModel> Reviews { get; set; } = [];

        public List<string> Tags { get; set; } = [];
        public List<CategoryModel> Categories { get; set; } = [];
    }

    public class AddPlaceRequest
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Address { get; set; } = "";
        public string Country { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Website { get; set; } = "";
        public string OpeningHours { get; set; } = "";
        public EPlaceStatus Status { get; set; } = EPlaceStatus.None;
        
        public List<IFormFile> Images { get; set; } = [];
        public List<string> Tags { get; set; } = [];
        public List<Guid> CategoryIds { get; set; } = [];
    }

    public class UpdatePlaceRequest
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Address { get; set; } = "";
        public string Country { get; set; } = "";
       
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string OpeningHours { get; set; } = "";
        public EPlaceStatus Status { get; set; } = EPlaceStatus.None;

        public List<IFormFile> Images { get; set; } = []; // new Image
        public List<string> DeleteImages { get; set; } = []; // old images to remove

        public List<string> Tags { get; set; } = [];
        public List<Guid> CategoryIds { get; set; } = [];
    }

    public class DeleteManyPlacesRequest
    {
        public List<string> Ids { get; set; } = [];
    }

    public class PlaceSearchRequest
    {
        public string? Keyword { get; set; }          // name + address
        public string? Address { get; set; }
        public string? Country { get; set; }

        public List<Guid>? CategoryIds { get; set; }

        public double? MinRating { get; set; }        // >= 1,2,3,4
        public double? MaxRating { get; set; }

        public int? MinViews { get; set; }
        public int? MaxViews { get; set; }

        public EPlaceStatus? Status { get; set; }

        public List<string>? Tags { get; set; }

        public string? SortBy { get; set; }           // rating, views, newest
        public bool SortDesc { get; set; } = true;

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
