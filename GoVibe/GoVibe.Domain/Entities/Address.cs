namespace GoVibe.Domain.Entities
{
    public class Address : Entity
    {
        public string Street { get; set; } = "";
        public string Ward { get; set; } = "";
        public string District { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public ICollection<Place> Places { get; set; } = [];
    }
}