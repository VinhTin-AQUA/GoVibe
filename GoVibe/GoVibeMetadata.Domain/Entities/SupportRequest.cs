using GoVibeMetadata.Domain.Enums;

namespace GoVibeMetadata.Domain.Entities
{
    public class SupportRequest : Entity
    {
        public ESupportType SupportType { get; set; }
        public string? AdminNote { get; set; }
        public ESupportResolveType ESupportResolveType { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }
}
