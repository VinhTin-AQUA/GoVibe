using Microsoft.AspNetCore.Identity;

namespace GoVibeAuth.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? GoogleId { get; set; }
    }
}
