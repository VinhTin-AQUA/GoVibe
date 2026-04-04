namespace GoVibe.Domain.Entities
{
    public class Category : Entity
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public ICollection<PlaceCategory> PlaceCategories { get; set; } = [];
    }
}