namespace GoVibeFavorite.Domain.Entities
{
    public class Favorite
    {
        public Guid UserId { get; set; }
        //public ApplicationUser? User { get; set; }

        public Guid PlaceId { get; set; }

        //public Place? Place { get; set; }
    }
}
