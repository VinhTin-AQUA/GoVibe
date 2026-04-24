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

        public async Task<GooglePayloadResult?> VerifyGoogleToken(ExternalAuth externalAuth)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _googleSettings.ClientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.Credential, settings);
            var googlePayloadResult = new GooglePayloadResult
            {
                Email = payload.Email,
                Name = payload.Name,
                Subject = payload.Subject, // Google user id
                Picture = payload.Picture
            };
            return googlePayloadResult;
        }
    }
}