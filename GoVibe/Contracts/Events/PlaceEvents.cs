using Contracts.Models;

namespace Contracts.Places
{
    public class PlaceEvents
    {

    }

    public class PlaceCreatedEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string Country { get; set; } = "";

        public int TotalViews { get; set; }
        public double TotalRating { get; set; }
        public int TotalReviews { get; set; }
        public ICollection<CategoryModel> Categories { get; set; } = [];
    }
}
