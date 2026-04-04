namespace GoVibeAuth.Domain.Entities
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }


        public string Email { get; set; } = string.Empty;

 
        public string? FullName { get; set; }

        public string? AvatarUrl { get; set; }

   
        public string? GoogleId { get; set; }


        public string Role { get; set; } = "user";

        //[Column(TypeName = "jsonb")]
        public Dictionary<string, object>? Preferences { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}
