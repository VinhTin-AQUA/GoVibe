using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;

namespace GoVibe.Infrastructure.Repositories.Amenities
{
    public interface IAmenityCommandRepository : ICommandRepository<Amenity>
    {
        
    }
    
    public class AmenityCommandRepository(AppDbContext context) : CommandRepository<Amenity>(context), IAmenityCommandRepository
    {
        
    }
}