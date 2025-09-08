using FluentValidation;
using Serilog;
using System.Net;
using System.Text.Json;

namespace TaskManager.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

                Log.Warning("Validation failed: {@Errors}", errors);

                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Validation failed",
                    Errors = errors,
                    TraceId = context.TraceIdentifier,
                    Timestamp = DateTime.UtcNow
                }));

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                Log.Error(ex, "Unhandled exception");

                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An unexpected error occurred",
                    TraceId = context.TraceIdentifier,
                    Timestamp = DateTime.UtcNow
                }));
            }
        }
    }
}
