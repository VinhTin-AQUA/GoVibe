using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoVibe.API.Controllers.Common
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
