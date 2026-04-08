using GoVibe.API.Models.Categories;
using GoVibe.API.Models.Places;

namespace GoVibe.API.Models.Statistics
{
    public class StatisticModel
    {
    }

    public class StatisticDateRangeQuery
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public (DateTime from, DateTime to) GetRange()
        {
            var to = ToDate ?? DateTime.UtcNow;
            var from = FromDate ?? to.AddDays(-30);

            return (from, to);
        }
    }

    public class StatisticOverView
    {
        public int TotalPlaces { get; set; }
        public int TotalCategories { get; set; }
        public int TotalReviews { get; set; }
        public int TotalReviewImage { get; set; }
        public int TotalPlaceImage { get; set; }
        public double AverageRating { get; set; }
    }

    public class RatingDistribution
    {
        public int Rating { get; set; }
        public int Count { get; set; }
    }

    public class PlaceGrowth
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Count { get; set; }
    }

    public class ReviewGrowth
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Count { get; set; }
    }

    public class TopCategory
    {
        public string CategoryName { get; set; } = "";
        public double AvgRating { get; set; }
    }

    public class StatisticSummary
    {
        public List<PlaceModel> TopRatedPlaces { get; set; } = [];
        public List<PlaceModel> MostViewedPlaces { get; set; } = [];
        public List<CategoryStats> PlacesPerCategory { get; set; } = [];
    }
}
