using GoVibeAuth.API.Controllers.Common;
using GoVibeAuth.API.Models.Auths;
using GoVibeAuth.API.Models.Common;
using GoVibeAuth.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoVibeAuth.API.Controllers.Auths
{
    public class GoogleAuthController : ControllerBaseApi
    {
        private readonly AuthService _authService;

        public GoogleAuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login-with-gg")]
        public async Task<IActionResult> LoginWithGoogle(ExternalAuth externalAuth)
        {
            if (string.IsNullOrEmpty(externalAuth.Credential))
                return BadRequest("Missing token");
            
            var jwt = await _authService.LoginWithGoogleAsync(externalAuth);
            return Ok(new ApiResponse<object>()
            {
                Item = new AuthModel
                {
                    Jwt = jwt
                }
            });
        }
    }
}