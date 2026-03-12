namespace GoVibe.Domain.Entities
{
    public class PlaceImage: Entity
    {
        public Guid PlaceId { get; set; }
        public Place? Place { get; set; }
        
        public string ImageUrl { get; set; } = "";
    }
}