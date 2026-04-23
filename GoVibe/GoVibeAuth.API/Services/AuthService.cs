using GoVibeAuth.API.Models.Auths;
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
            JwtService  jwtService, 
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
            var userInfo = await _googleService.VerifyGoogleToken(externalAuth);
            if (userInfo == null)
            {
                throw new ArgumentException("Login with Google Failed");
            }
            
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                throw new ArgumentException("Error loading external login info");
            }
            
            // check record in AspNetUserLogins
            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false);
            
            ApplicationUser? user;
            if (signInResult.Succeeded)
            {
                user = await _userManager.FindByLoginAsync(
                    info.LoginProvider,
                    info.ProviderKey);
            }
            else
            {
                user = await _userManager.FindByEmailAsync(userInfo.Email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = userInfo.Email,
                        Email = userInfo.Email,
                        EmailConfirmed = true
                    };

                    var createResult = await _userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                        throw new ArgumentException("Login with Google Failed");
                }

                // Link Google login
                await _userManager.AddLoginAsync(user, info);
            }

            if (user == null)
            {
                throw new ArgumentException("Login with Google Failed");
            }

            var jwt = await _jwtService.CreateJWT(user);
            return jwt;
        }
    }
}