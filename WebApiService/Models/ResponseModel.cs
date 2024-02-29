using System.Net;

namespace WebApiService.Models
{
    public class ResponseModel
    {
        public int Id { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
