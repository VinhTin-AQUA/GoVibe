namespace GoVibe.Domain.Entities
{
    public class Amenity : Entity
    {
        public string Name { get; set; } = "";
        public string Icon { get; set; } = "";
        public ICollection<PlaceAmenity> PlaceAmenities { get; set; } = [];
    }
}