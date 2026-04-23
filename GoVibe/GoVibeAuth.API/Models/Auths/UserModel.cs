namespace GoVibeAuth.API.Models.Auths
{
    public class UserModel
    {
        public string Email {get; set;} = string.Empty;
        public string Name {get; set;} = string.Empty;
        public string GoogleId { get; set; } = string.Empty;
    }
}