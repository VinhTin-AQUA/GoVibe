using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.Amenities
{
    public interface IAmenityQueryRepository : IQueryRepository<Amenity>
    {
        
    }
    
    public class AmenityQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<Amenity>(contextFactory), IAmenityQueryRepository
    {
        
    }
}