using System.Text;
using Lab03_MinimalAPI.Data;
using Lab03_MinimalAPI.Middleware;
using Lab03_MinimalAPI.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ======================================
// 🔹 Serilog – konfiguracja logowania
// ======================================
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration));

// ======================================
// 🔹 Baza danych – SQL / InMemory dla testów
// ======================================
if (builder.Environment.EnvironmentName == "Testing")
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("TestDb"));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// ======================================
// 🔹 AutoMapper
// ======================================
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ======================================
// 🔹 JWT – konfiguracja i logowanie błędów
// ======================================
var jwtSection = builder.Configuration.GetSection("Jwt");

// Priorytet: najpierw zmienna środowiskowa, potem plik konfiguracyjny
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? jwtSection["Key"];

if (string.IsNullOrEmpty(jwtKey))
{
    // Logowanie ostrzeżenia po stronie serwera
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("⚠️  JWT key not found in configuration or environment variables!");
    Console.ResetColor();

    // Awaryjny klucz (np. dla testów)
    jwtKey = "temporary_dev_key_123456789";
}

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

// ======================================
// 🔹 API Versioning
// ======================================
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// ======================================
// 🔹 Swagger
// ======================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lab03 Minimal API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

// ======================================
// 🔹 HTTPS – wymuszanie połączeń szyfrowanych
// ======================================
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 5001; // domyślny port HTTPS w ASP.NET
});

// ======================================
// 🔹 Budowa aplikacji
// ======================================
var app = builder.Build();

// ======================================
// 🔹 Middleware
// ======================================
app.UseMiddleware<ErrorLoggingMiddleware>();
app.UseSerilogRequestLogging();

app.UseHttpsRedirection(); // 🔒 automatyczne przekierowanie HTTP → HTTPS

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lab03 Minimal API v1");
    c.RoutePrefix = string.Empty; // <-- dzięki temu Swagger będzie pod rootem (/)
});

// ======================================
// 🔹 Endpointy techniczne
// ======================================
app.MapGet("/api/v1/health", () => Results.Ok(new { status = "Healthy" }));
app.MapGet("/api/v1/hello/{name}", (string name) => Results.Ok($"Hello, {name}!")).AllowAnonymous();

// ======================================
// 🔹 Endpointy domenowe
// ======================================
Lab03_MinimalAPI.Endpoints.UserEndpoints.Map(app);
Lab03_MinimalAPI.Endpoints.TaskEndpoints.Map(app);
Lab03_MinimalAPI.Endpoints.ReportEndpoints.Map(app);
Lab03_MinimalAPI.Endpoints.RoleEndpoints.Map(app);

// ======================================
// 🔹 Uruchomienie aplikacji
// ======================================
app.Run();

public partial class Program { }


// plik do rozbudowy/modyfikacji w przyszłości