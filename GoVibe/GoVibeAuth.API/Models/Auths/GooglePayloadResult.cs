namespace GoVibeAuth.API.Models.Auths
{
    public class GooglePayloadResult
    {
        public string Email {get; set;} = string.Empty;
        public string Name {get; set;} = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
    }
}