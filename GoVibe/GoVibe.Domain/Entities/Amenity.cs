using GoVibe.Domain.Enums;

namespace GoVibe.Domain.Entities
{
    public class Amenity : Entity
    {
        public string Name { get; set; } = "";
        public EAmenityStatus Status { get; set; } = EAmenityStatus.None;
        public ICollection<PlaceAmenity> PlaceAmenities { get; set; } = [];
    }
}