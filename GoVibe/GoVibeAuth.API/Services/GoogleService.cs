using Google.Apis.Auth;
using GoVibeAuth.API.Configs;
using GoVibeAuth.API.Models.Auths;

namespace GoVibeAuth.API.Services
{
    public class GoogleService
    {
        private readonly GoogleSettings _googleSettings;

        public GoogleService(GoogleSettings googleSettings)
        {
            _googleSettings = googleSettings;
        }

        public async Task<UserModel?> VerifyGoogleToken(ExternalAuth externalAuth)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _googleSettings.ClientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.Credential, settings);
            
            var userInfo = new UserModel
            {
                Email = payload.Email,
                Name = payload.Name,
                GoogleId = payload.Subject // Google user id
            };
            return userInfo;
        }
    }
}