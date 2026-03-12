using GoVibe.Domain.Enums;

namespace GoVibe.API.Models.Amenities
{
    public class AmenityModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public EAmenityStatus Status { get; set; } = EAmenityStatus.None;
        public DateTime UpdatedAt { get; set; }
    }

    public class AddAmenityRequest
    {
        public string Name { get; set; } = "";
        public EAmenityStatus Status { get; set; } = EAmenityStatus.None;
    }

    public class UpdateAmenityRequest
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public EAmenityStatus Status { get; set; } = EAmenityStatus.None;
    }

    public class DeleteManyAmenitiesRequest
    {
        public List<string> Ids { get; set; } = [];
    }
}
