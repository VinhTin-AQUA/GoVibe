
using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.Categories
{    
    public interface ICategoryQueryRepository : IQueryRepository<Category>
    {
        Task<(List<Category>, int)> GetAllPagination(int pageIndex = 0, int pageSize = 50);
    }
    
    public class CategoryQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<Category>(contextFactory), ICategoryQueryRepository
    {
        public async Task<(List<Category>, int)> GetAllPagination(int pageIndex = 0, int pageSize = 50)
        {
            var r = await GetPagedAsync(pageIndex, pageSize);
            var total = await CountAsync(x => true);
            return (r, total);
        }
    }
}
