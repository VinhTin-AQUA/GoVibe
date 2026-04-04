using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.Places
{
    public interface IPlaceCategoryQueryRepository : IQueryRepository<PlaceCategory>
    {
    }
    
    public class PlaceCategoryQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<PlaceCategory>(contextFactory), IPlaceCategoryQueryRepository
    {
    }
}