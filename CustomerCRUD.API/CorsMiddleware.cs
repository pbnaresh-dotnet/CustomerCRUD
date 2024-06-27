using System.Diagnostics;

namespace CustomerCRUD.API
{

    internal class CorsMiddleware
    {
        private readonly RequestDelegate _next;

        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            context.Response.OnStarting(() =>
            {
                //context.Response.Headers["Access-Control-Allow-Origin"] = "https://localhost:7096";
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
    public static class CorsMiddlewareExtensions
    {
        public static IApplicationBuilder EnableCustomCors(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorsMiddleware>();
        }
    }
}
