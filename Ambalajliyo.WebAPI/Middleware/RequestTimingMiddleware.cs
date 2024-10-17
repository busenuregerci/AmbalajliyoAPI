using Serilog;
using System.Diagnostics;

namespace Ambalajliyo.WebAPI.Middleware
{
    public class RequestTimingMiddleware //API yanıt sürelerini loglamak için bir middleware oluşturabilir 
    {
        private readonly RequestDelegate _next;

        public RequestTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            var requestPath = context.Request.Path; // İstek yapılan endpoint
            var requestMethod = context.Request.Method; // GET, POST vb.
            var queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty; // Query string varsa
            var statusCode = context.Request.HttpContext.Response.StatusCode;
            Log.Information($"API İsteği {requestMethod} {requestPath}{queryString} {elapsedMilliseconds} ms içinde tamamlandı. Durum Kodu: {statusCode}");
        }
    }
}