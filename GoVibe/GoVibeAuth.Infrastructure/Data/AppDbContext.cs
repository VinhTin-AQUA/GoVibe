using Microsoft.EntityFrameworkCore;

namespace GoVibeAuth.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        // public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        // {
        //     var entries = ChangeTracker.Entries<Entity>();
        //     var now = DateTime.UtcNow;
        //
        //     foreach (var entry in entries)
        //     {
        //         if (entry.State == EntityState.Added)
        //         {
        //             entry.Entity.CreatedAt = now;
        //             entry.Entity.UpdatedAt = now;
        //         }
        //
        //         if (entry.State == EntityState.Modified)
        //         {
        //             entry.Entity.UpdatedAt = now;
        //             entry.Property(x => x.CreatedAt).IsModified = false;
        //         }
        //     }
        //     return await base.SaveChangesAsync(cancellationToken);
        // }
    }
}