namespace GoVibeItinerary.Domain.Entities
{
    public class Itinerary : Entity
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //public User? User { get; set; }
        public ICollection<ItineraryItem> ItineraryItems { get; set; } = new List<ItineraryItem>();
    }
}
