
using System.Linq.Expressions;
using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using GoVibe.Infrastructure.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.Categories
{    
    public interface ICategoryQueryRepository : IQueryRepository<Category>
    {
        Task<(List<Category>, int)> GetAllPagination(string searchString = "", int pageIndex = 0, int pageSize = 50);
    }
    
    public class CategoryQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<Category>(contextFactory), ICategoryQueryRepository
    {
        public async Task<(List<Category>, int)> GetAllPagination(string searchString = "", int pageIndex = 0, int pageSize = 50)
        {
            QueryOptionsBuilder<Category> builder = new();
            Expression<Func<Category, bool>> predicate = x => true;
            
            if (!string.IsNullOrEmpty(searchString))
            {
                builder.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
                predicate= category => category.Name.ToLower().Contains(searchString.ToLower());
            }

            builder.AsNoTracking()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
            
            var r = await FilterAsync(builder.Build());
            var total = await CountAsync(predicate);
            return (r, total);
        }
    }
}
