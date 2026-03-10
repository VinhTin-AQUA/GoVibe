using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.PlaceAmenities
{
    public interface IPlaceAmenityQueryRepository : IQueryRepository<PlaceAmenity>
    {
        
    }
    
    public class PlaceAmenityQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<PlaceAmenity>(contextFactory), IPlaceAmenityQueryRepository
    {
        
    }
}