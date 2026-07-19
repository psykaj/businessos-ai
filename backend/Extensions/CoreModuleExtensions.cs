using backend.Modules.ApiKeys.Interfaces;
using backend.Modules.ApiKeys.Services;
using backend.Modules.AuditLogs.Interfaces;
using backend.Modules.AuditLogs.Services;
using backend.Modules.Organization.Interfaces;
using backend.Modules.Organization.Services;
using backend.Modules.Team.Interfaces;
using backend.Modules.Team.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using backend.Middleware;

namespace backend.Extensions;

public static class CoreModuleExtensions
{
    public static IServiceCollection AddCoreModules(this IServiceCollection services)
    {
        // Organization
        services.AddScoped<IOrganizationService, OrganizationService>();
        
        // Team
        services.AddScoped<ITeamService, TeamService>();
        
        // ApiKeys
        services.AddScoped<IApiKeyService, ApiKeyService>();
        
        // AuditLogs
        services.AddScoped<IAuditLogService, AuditLogService>();

        // White Label Platform
        services.AddScoped<backend.Modules.Media.Interfaces.IMediaService, backend.Modules.Media.Services.LocalMediaService>();
        
        services.AddScoped<backend.Modules.Branding.Repositories.IBrandRepository, backend.Modules.Branding.Repositories.BrandRepository>();
        services.AddScoped<backend.Modules.Branding.Interfaces.IBrandService, backend.Modules.Branding.Services.BrandService>();

        services.AddScoped<backend.Modules.CustomDomains.Repositories.ICustomDomainRepository, backend.Modules.CustomDomains.Repositories.CustomDomainRepository>();
        services.AddScoped<backend.Modules.CustomDomains.Interfaces.ICustomDomainService, backend.Modules.CustomDomains.Services.CustomDomainService>();

        services.AddScoped<backend.Modules.LandingPages.Repositories.ILandingPageRepository, backend.Modules.LandingPages.Repositories.LandingPageRepository>();
        services.AddScoped<backend.Modules.LandingPages.Interfaces.ILandingPageService, backend.Modules.LandingPages.Services.LandingPageService>();

        services.AddScoped<backend.Modules.Themes.Repositories.IThemeRepository, backend.Modules.Themes.Repositories.ThemeRepository>();
        services.AddScoped<backend.Modules.Themes.Interfaces.IThemeService, backend.Modules.Themes.Services.ThemeService>();

        services.AddScoped<backend.Modules.SEO.Repositories.ISEORepository, backend.Modules.SEO.Repositories.SEORepository>();
        services.AddScoped<backend.Modules.SEO.Interfaces.ISEOService, backend.Modules.SEO.Services.SEOService>();

        // RBAC Authorization Setup
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        return services;
    }
}
