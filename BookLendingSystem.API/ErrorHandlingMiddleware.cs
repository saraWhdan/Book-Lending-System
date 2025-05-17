namespace BookLendingSystem.API
{
    // Middlewares/ErrorHandlingMiddleware.cs
    using Microsoft.AspNetCore.Http;
    using Serilog;
    using System.Net;
    using System.Text.Json;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Continue the pipeline
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unhandled exception occurred while processing request: {Path}", context.Request.Path);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    status = context.Response.StatusCode,
                    error = "An unexpected error occurred.",
                    path = context.Request.Path,
                    details = ex.Message // You can remove this in production
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
            }
        }
    }

}
