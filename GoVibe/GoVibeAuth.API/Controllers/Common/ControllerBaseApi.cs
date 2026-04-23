using Microsoft.AspNetCore.Mvc;

namespace GoVibeAuth.API.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerBaseApi : ControllerBase
    {
        public ControllerBaseApi()
        {
            
        }
    }
}