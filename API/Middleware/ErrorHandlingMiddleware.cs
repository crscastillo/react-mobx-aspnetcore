using System;
using System.Net;
using System.Threading.Tasks;
using Application.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ErrorHandlingMiddleware> logger)
        {
            object errors = null;

            if (ex is RestException)
            {
                logger.LogError(ex, "REST ERROR");
                errors = ((RestException)ex).Errors;
                context.Response.StatusCode = (int)((RestException)ex).Code;
            }
            else 
            {
                logger.LogError(ex, "SERVER ERROR");
                errors = string.IsNullOrWhiteSpace(ex.Message) ? "Error" : ex.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.Response.ContentType = "application/json";
            if (errors != null)
            {
                var result = JsonConvert.SerializeObject(new
                {
                    errors
                });

                await context.Response.WriteAsync(result);
            }
        }
    }
}