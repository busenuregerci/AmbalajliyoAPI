using Ambalajliyo.BLL.Interfaces;
using Ambalajliyo.BLL.Services;
using Ambalajliyo.DAL.AbstractRepository;
using Ambalajliyo.DAL.ConcreteRepository;
using Ambalajliyo.DAL.Context;
using Ambalajliyo.DAL.Entities;
using Ambalajliyo.WebAPI.ActionFilters;
using Ambalajliyo.WebAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text;
using System.Threading.RateLimiting;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Serilog konfigürasyonu
        var connectionString = builder.Configuration.GetConnectionString("LogConnection");
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.MSSqlServer(
                connectionString: connectionString,
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    AutoCreateSqlTable = true
                },
                restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        // Veritabaný konfigürasyonu
        builder.Services.AddDbContext<AmbalajliyoDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddDbContext<AmbalajliyoLogDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("LogConnection")));

        // Identity konfigürasyonu
        builder.Services.AddIdentity<AmbalajliyoUser, AmbalajliyoRole>()
            .AddEntityFrameworkStores<AmbalajliyoDbContext>()
            .AddDefaultTokenProviders();

        // JWT Authentication konfigürasyonu (Tekrar eden kýsmý kaldýrdýk)
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Token ayarlarýný belirleyelim
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true, // JWT token'ý veren kiþiyi doðrula
                    ValidateAudience = true, // JWT token'ý kullanan kiþiyi doðrula
                    ValidateLifetime = true, // JWT token'ýn süresini doðrula
                    ValidateIssuerSigningKey = true, // JWT token'ýn imza key'ini doðrula
                    RequireExpirationTime = true,

                    ValidIssuer = builder.Configuration["JwtTokenSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtTokenSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtTokenSettings:Key"]))
                };
            });            

            // Rate limiting yapýlandýrmasý
            builder.Services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    // Swagger dokümantasyonu için rate limiting'i devre dýþý býrakýyoruz.
                    // Çünkü Swagger UI genellikle dokümantasyon amaçlýdýr ve burada rate limiting'in uygulanmasý gereksizdir.
                    var path = httpContext.Request.Path.ToString().ToLower();
                    if (path.StartsWith("/swagger") || path.StartsWith("/v1/swagger"))
                    {
                        // Swagger isteði olduðunda rate limiter devre dýþý býrakýlýr.
                        return RateLimitPartition.GetNoLimiter<string>(httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString());
                    }

                    // Diðer tüm istekler için rate limiting uygulanýr.
                    // Kullanýcý kimliði ya da host bilgisine göre partition key belirlenir.
                    return RateLimitPartition.GetFixedWindowLimiter(
                        // Kullanýcý kimliði varsa partition key olarak kullanýlýr, yoksa host bilgisi kullanýlýr.
                        partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),

                        // Belirli bir zaman diliminde (Fixed Window) istekleri sýnýrlamak için limiter yapýlandýrýlýyor.
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true, // Süre dolduðunda limit otomatik olarak sýfýrlanýr.
                            PermitLimit = 20, // 1 saniyede en fazla 20 istek yapýlabilir.
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst, // Sýraya alýnan istekler ilk gelen ilk çýkar prensibine göre iþlenir.
                            Window = TimeSpan.FromSeconds(1) // 1 saniyelik zaman penceresi belirlenir, bu süre içinde en fazla 2 istek yapýlabilir.                            
                        });
                });

                // Rate limiting ihlali durumunda geri dönüþ
                options.OnRejected = async (context, token) =>
                {
                    Log.Warning("Rate limit aþýldý: {User} - {Host}",
                        context.HttpContext.User.Identity?.Name ?? "Anonim",
                        context.HttpContext.Request.Headers.Host.ToString());

                    context.HttpContext.Response.StatusCode = 429;

                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        context.HttpContext.Response.Headers["Retry-After"] = retryAfter.TotalSeconds.ToString();
                        await context.HttpContext.Response.WriteAsJsonAsync(
                            new
                            {
                                Message = $"Çok fazla istekde bulundunuz. Lütfen sonra tekrar deneyin {retryAfter.TotalMinutes} dakika sonra.",
                                RetryAfter = retryAfter.TotalSeconds
                            }, cancellationToken: token);
                    }
                    else
                    {
                        await context.HttpContext.Response.WriteAsJsonAsync(
                            new
                            {
                                Message = "Çok fazla istekde bulundunuz. Lütfen sonra tekrar deneyin."
                            }, cancellationToken: token);
                    }
                };
            });

        // Controller ve filtre kayýtlarý
        builder.Services.AddControllers(config =>
        {
            config.Filters.Add<PerformanceLoggingFilter>(); // Global filtre kaydý
        });

        // Authorization
        builder.Services.AddAuthorization();

        // Swagger konfigürasyonu
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Dependency Injection (DI) kayýtlarý
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<ILogService, LogService>();
        builder.Services.AddScoped<IPostService, PostService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IFaqService, FaqService>();
        builder.Services.AddScoped<ICatalogService, CatalogService>();

        // Health checks konfigürasyonu
        builder.Services.AddHealthChecks()
            .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), name: "sqlserver");

        var app = builder.Build();

        // Health check endpoint
        app.MapHealthChecks("/health");

        // Middleware'ler
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<RequestTimingMiddleware>();

        // Geliþtirme ortamýnda Swagger kullanýmý
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Rate Limiter'ý kullan
        app.UseRateLimiter();

        app.UseHttpsRedirection();

        app.UseAuthentication(); // Authentication kullanýmý
        app.UseAuthorization();  // Authorization kullanýmý

        app.MapControllers();

        app.Run();
    }
}
