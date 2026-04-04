namespace GoVibeItinerary.Domain.Entities
{
    public class ItineraryItem : Entity
    {
        public int ItineraryId { get; set; }
        public int? PlaceId { get; set; }
        public int? VisitOrder { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public Itinerary? Itinerary { get; set; }
        //public Place? Place { get; set; }
    }
}
