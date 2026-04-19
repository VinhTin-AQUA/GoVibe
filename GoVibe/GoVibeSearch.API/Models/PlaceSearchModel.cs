namespace GoVibeSearch.API.Models
{
    public class PlaceSearchModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string Country { get; set; } = "";
        public int TotalViews { get; set; }
        public double TotalRating { get; set; }
        public int TotalReviews { get; set; }
        public float AverageRating { get; set; }
        public string Thumbnail { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
        public string Status { get; set; } = "";
        public List<string>? Tags { get; set; }
        public ICollection<CategorySearchModel> Categories { get; set; } = [];
    }

    public class PlaceSearchRequest
    {
        public string? Keyword { get; set; }          // name + address
        public string? Country { get; set; }
        public List<Guid>? CategoryIds { get; set; }
        public double? MinRating { get; set; }        // >= 1,2,3,4
        public int? MinViews { get; set; }
        public string Status { get; set; } = "";
        public List<string>? Tags { get; set; }
        public string? SortBy { get; set; }           // rating, views
        public bool? SortDesc { get; set; } 

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
