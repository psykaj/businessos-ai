using System.Text;
using backend.Authentication;
using backend.Configurations;
using backend.Persistence;
using backend.Extensions;
using backend.Modules.QRCode.Extensions;
using backend.Modules.Analytics.Extensions;
using backend.Modules.Billing.Extensions;
using backend.Modules.CRM.Extensions;
using backend.Modules.Workflow.Extensions;
using backend.Modules.BusinessIntelligence.Extensions;
using backend.Seed;
using backend.Interfaces;
using backend.Middleware;
using backend.Services;
using backend.Validators;
using backend.Modules.Notifications.Hubs;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ─── Configuration ────────────────────────────────────────────────────────────
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>()!;

// ─── Database ─────────────────────────────────────────────────────────────────
builder.Services.AddDatabaseInfrastructure(builder.Configuration);

// ─── Authentication & Authorization ───────────────────────────────────────────
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ValidateIssuer   = true,
            ValidIssuer      = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience    = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew        = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// ─── CORS ─────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .SetIsOriginAllowed(origin => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// ─── Application Services ─────────────────────────────────────────────────────
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpClient<IWhatsAppService, WhatsAppService>();
builder.Services.AddScoped<IWhatsAppService, WhatsAppService>();

// ─── Validators ───────────────────────────────────────────────────────────────
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

// ─── Controllers & OpenAPI ────────────────────────────────────────────────────
builder.Services.AddQRCodeModule();
builder.Services.AddAnalyticsModule();
builder.Services.AddBillingModule();
builder.Services.AddCrmModule();
builder.Services.AddWorkflowModule();
builder.Services.AddBusinessIntelligenceModule();
builder.Services.AddCoreModules();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();
builder.Services.AddSignalR();

// ─── Build ────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ─── Middleware Pipeline ──────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

// ─── Auto-apply Migrations & Seed on Startup ──────────────────────────────────
try
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    if (app.Environment.IsDevelopment())
    {
        await SeedData.InitializeAsync(app.Services);
        logger.LogInformation("Database migrations and seed data applied successfully.");
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogWarning(
        "Could not connect to the database on startup: {Message}. " +
        "Ensure PostgreSQL is running. The API will start but database operations will fail.",
        ex.Message);
}


app.Run();
