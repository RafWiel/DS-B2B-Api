using System.Net;

namespace WebApiService.Models
{
    public class IdResponseModel
    {
        public int Id { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
