using System.Linq.Expressions;
using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using GoVibe.Infrastructure.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.PlaceCategories
{
    public interface IPlaceCategoryQueryRepository : IQueryRepository<PlaceCategory>
    {
        Task<List<PlaceCategory>> GetListByPlaceIdIncludeCategory( Guid placeId);
    }
    
    public class PlaceCategoryQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<PlaceCategory>(contextFactory), IPlaceCategoryQueryRepository
    {
        public async Task<List<PlaceCategory>> GetListByPlaceIdIncludeCategory(Guid placeId)
        {
            QueryOptionsBuilder<PlaceCategory> builder = new();
            builder.Where(x => x.PlaceId == placeId);
            builder.Include(x => x.Category);
            builder.AsNoTracking();
            
            var r = await FilterAsync(builder.Build());
            return r;
        }
    }
}