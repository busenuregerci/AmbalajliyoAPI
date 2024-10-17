using Microsoft.AspNetCore.Mvc;
using System.Net;
using Serilog;

namespace Ambalajliyo.WebAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Bir sonraki middleware'e devam et
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Hata meydana geldiğinde logla ve özel bir yanıt dön
                Log.Error(ex, "İstek işlenirken işlenmeyen bir özel durum oluştu.");

                // ProblemDetails ile hata yanıtı dön
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // HTTP yanıt kodu
            var statusCode = (int)HttpStatusCode.InternalServerError;

            // ProblemDetails oluştur
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = "Sunucu Hatası",
                Detail = "İstek işlenirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.",
                Instance = context.Request.Path
            };

            // İçerik tipi ve durum kodu ayarla
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            // ProblemDetails'i JSON formatında döndür
            return context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}