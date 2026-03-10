using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;

namespace GoVibe.Infrastructure.Repositories.PlaceAmenities
{
    public interface IPlaceAmenityCommandRepository : ICommandRepository<PlaceAmenity>
    {
        
    }
    
    public class PlaceAmenityCommandRepository(AppDbContext context) : CommandRepository<PlaceAmenity>(context), IPlaceAmenityCommandRepository
    {
        
    }
}