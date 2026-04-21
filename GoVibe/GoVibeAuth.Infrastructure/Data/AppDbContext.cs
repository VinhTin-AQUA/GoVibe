using GoVibeAuth.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoVibeAuth.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (string.IsNullOrEmpty(tableName) == false && tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName[6..]);
                }
            }
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