using Asp.Versioning;
using LunarCalendar.Api.Data;
using LunarCalendar.Api.Services;
using LunarCalendar.Api.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Configure Swagger with comprehensive documentation
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Vietnamese Lunar Calendar API",
        Description = "A comprehensive API for Vietnamese lunar calendar calculations, holidays, and cultural information.",
        Contact = new OpenApiContact
        {
            Name = "LunarCalendar Support",
            Email = "support@lunarcalendar.app"
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Add JWT authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Enable XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Add response caching
builder.Services.AddResponseCaching();

// Add memory cache for service-level caching
builder.Services.AddMemoryCache();

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Database configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// CORS configuration
// Note: Mobile apps don't typically need CORS since they're not browser-based
// However, if you plan to add a web interface, configure specific origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("MobileAppPolicy", policy =>
    {
        // For mobile apps, CORS is not strictly necessary
        // But we configure it for potential web clients in the future
        if (builder.Environment.IsDevelopment())
        {
            // Allow all in development for testing
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            // In production, restrict to specific origins if you add a web interface
            // For now, allow any origin but this should be restricted when you deploy
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
            // TODO: Replace with specific origin when production web interface is added
            // policy.WithOrigins("https://lunarcalendar.app")
        }
    });
});

// Register Core services (shared library)
builder.Services.AddSingleton<LunarCalendar.Core.Services.ILunarCalculationService, LunarCalendar.Core.Services.LunarCalculationService>();
builder.Services.AddSingleton<LunarCalendar.Core.Services.IHolidayCalculationService, LunarCalendar.Core.Services.HolidayCalculationService>();

// Register API services (wrappers that add extra features)
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<ILunarCalendarService, LunarCalendarService>();
builder.Services.AddSingleton<IHolidayService, HolidayService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Add exception handling middleware first
app.UseExceptionHandler();

// Enable Swagger in all environments for MVP (can restrict later)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "LunarCalendar API v1");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "Vietnamese Lunar Calendar API";
    options.DisplayRequestDuration();
});

// Security headers
app.Use(async (context, next) =>
{
    // Prevent clickjacking
    context.Response.Headers.Append("X-Frame-Options", "DENY");

    // Prevent MIME type sniffing
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

    // XSS protection (legacy but still useful)
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");

    // Referrer policy
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");

    // Content Security Policy (basic for API)
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'");

    // Permissions policy
    context.Response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=(), camera=()");

    await next();
});

app.UseHttpsRedirection();
app.UseCors("MobileAppPolicy");
app.UseResponseCaching();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Enhanced health check endpoint with detailed information
app.MapGet("/health", () =>
{
    var health = new
    {
        status = "Healthy",
        timestamp = DateTime.UtcNow,
        version = "1.0.0",
        environment = app.Environment.EnvironmentName,
        uptime = Environment.TickCount64 / 1000 // seconds since startup
    };
    return Results.Ok(health);
})
    .WithName("HealthCheck")
    .WithTags("System")
    .WithOpenApi(operation =>
    {
        operation.Summary = "Health Check";
        operation.Description = "Returns the health status of the API and basic system information.";
        return operation;
    });

// API info endpoint
app.MapGet("/", () => Results.Ok(new
{
    name = "Vietnamese Lunar Calendar API",
    version = "1.0.0",
    description = "API for lunar calendar calculations and Vietnamese holidays",
    documentation = "/swagger",
    endpoints = new
    {
        health = "/health",
        swagger = "/swagger",
        lunarDate = "/api/v1/lunardate/{year}/{month}/{day}",
        monthInfo = "/api/v1/lunardate/month/{year}/{month}",
        holidays = "/api/v1/holiday/year/{year}",
        monthHolidays = "/api/v1/holiday/month/{year}/{month}"
    }
}))
    .WithName("ApiInfo")
    .WithTags("System")
    .WithOpenApi(operation =>
    {
        operation.Summary = "API Information";
        operation.Description = "Returns basic information about the API and available endpoints.";
        return operation;
    })
    .ExcludeFromDescription(); // Don't show in Swagger UI

app.Run();
