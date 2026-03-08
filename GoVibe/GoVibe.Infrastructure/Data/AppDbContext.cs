using GoVibe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PlaceAmenity> PlaceAmenities { get; set; }
        public DbSet<PlaceImage> PlaceImages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewImage> ReviewImages { get; set; }
    }
}
