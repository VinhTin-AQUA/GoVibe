using GoVibeAuth.API.Models.Auths;
using GoVibeAuth.Domain.Constants;
using GoVibeAuth.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GoVibeAuth.API.Services
{
    public class AuthService
    {
        private readonly GoogleService _googleService;
        private readonly JwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        
        public AuthService(
            GoogleService googleService, 
            JwtService jwtService, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
        )
        {
            _googleService = googleService;
            _jwtService = jwtService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> LoginWithGoogleAsync(ExternalAuth externalAuth)
        {
            var payload = await _googleService.VerifyGoogleToken(externalAuth);
            if (payload == null)
            {
                throw new UnauthorizedAccessException("Login with Google Failed");
            }
     
            var providerKey = payload.Subject;

            // tìm user bằng login provider
            var user = await _userManager.FindByLoginAsync(LoginProviders.Google, providerKey);
            if (user == null)
            {
                // nếu chưa có → tìm bằng email
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = payload.Email,
                        Email = payload.Email,
                        EmailConfirmed = true,
                        AvatarUrl = payload.Picture,
                        FullName = payload.Name
                    };

                    await _userManager.CreateAsync(user);
                }

                // link google vào user
                var loginInfo = new UserLoginInfo(
                    LoginProviders.Google,
                    providerKey,
                    LoginProviders.Google
                );
                await _userManager.AddLoginAsync(user, loginInfo);
            }
            
            var jwt = await _jwtService.CreateJWT(user);
            return jwt;
        }
    }
}