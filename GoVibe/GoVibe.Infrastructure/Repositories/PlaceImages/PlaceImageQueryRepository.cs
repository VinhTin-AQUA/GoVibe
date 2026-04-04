using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using GoVibe.Infrastructure.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.PlaceImages
{
    public interface IPlaceImageQueryRepository : IQueryRepository<PlaceImage>
    {
        Task<List<PlaceImage>> GetListByPlaceId(Guid placeId);
    }
    
    public class PlaceImageQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<PlaceImage>(contextFactory), IPlaceImageQueryRepository
    {
        public async Task<List<PlaceImage>> GetListByPlaceId(Guid placeId)
        {
            QueryOptionsBuilder<PlaceImage> builder = new();
            builder.Where(x => x.PlaceId == placeId);   
            var r = await FilterAsync(builder.Build());
            return r;
        }
    }
}