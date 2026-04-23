using System.Net;

namespace GoVibeAuth.API.Models.Common
{
    public class ApiResponse<T> where T : new()
    {
        public string ErrorMessage { get; set; } = "";
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public T? Item { get; set; }
    }
}