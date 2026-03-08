namespace GoVibe.Domain.Entities
{
    public class PlaceAmenity : Entity
    {
        public Guid PlaceId { get; set; }

        public Place? Place { get; set; }

        public Guid AmenityId { get; set; }

        public Amenity? Amenity { get; set; }
    }
}