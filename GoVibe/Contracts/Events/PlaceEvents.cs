using System.Runtime.InteropServices.JavaScript;
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
        public float AverageRating { get; set; }
        public string Status { get; set; } = "";
        public string Thumbnail { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
        public List<string>? Tags { get; set; }
        public ICollection<CategoryOfPlaceEvent> Categories { get; set; } = [];
    }
}
