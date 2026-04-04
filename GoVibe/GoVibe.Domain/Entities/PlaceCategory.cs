namespace GoVibe.Domain.Entities
{
    public class PlaceCategory : Entity
    {
        public int PlaceId { get; set; }
        public Place? Place { get; set; }

        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
