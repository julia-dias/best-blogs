using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Service.Exceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                httpContext.Response.ContentType = "application/json";
                var statusCode = DefineStatusCode(ex);
                httpContext.Response.StatusCode = statusCode;

                if (statusCode != (int)HttpStatusCode.InternalServerError)
                {
                    await httpContext.Response
                        .WriteAsync(JsonSerializer.Serialize(new { Message = ex.Message }));
                }
            }
        }

        private static int DefineStatusCode(Exception ex) =>
        ex switch
        {
            EntityNotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };
    }
}
