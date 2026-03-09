
using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using GoVibe.Infrastructure.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.Categories
{    
    public interface ICategoryQueryRepository : IQueryRepository<Category>
    {
        Task<List<Category>> GetByMarkAsync();
    }
    
    public class CategoryQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<Category>(contextFactory), ICategoryQueryRepository
    {
        public async Task<List<Category>> GetByMarkAsync()
        {
            QueryOptionsBuilder<Category> builder = new();
            // builder.Where(x => x.Mark == mark);
            var r = await FilterAsync(builder.Build());
            return r;
        }
    }
}