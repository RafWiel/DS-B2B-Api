using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace WebApiService.Middlewares
{
    public class GlobalExceptionsMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionsMiddleware> _logger;
        public GlobalExceptionsMiddleware(ILogger<GlobalExceptionsMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

#if DEBUG

                throw;

#else
                
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problemDetails = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = "Internal server error occured"
                };

                var json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);

                context.Response.ContentType = "application/json";   

#endif
            }
        }
    }
}
