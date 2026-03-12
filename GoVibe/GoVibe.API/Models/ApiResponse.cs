using System.Net;

namespace GoVibe.API.Models
{
    public class ApiResponse<T> where T : new()
    {
        public string ErrorMessage { get; set; } = "";
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public T? Item { get; set; }
    }
}
