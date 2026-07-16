using System.Text;
using backend.Authentication;
using backend.Configurations;
using backend.Persistence;
using backend.Extensions;
using backend.Seed;
using backend.Interfaces;
using backend.Middleware;
using backend.Services;
using backend.Validators;
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
            .WithOrigins("http://localhost:3000", "https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// ─── Application Services ─────────────────────────────────────────────────────
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ─── Validators ───────────────────────────────────────────────────────────────
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

// ─── Controllers & OpenAPI ────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddOpenApi();

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
