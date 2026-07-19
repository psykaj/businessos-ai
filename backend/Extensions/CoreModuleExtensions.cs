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

        // RBAC Authorization Setup
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        return services;
    }
}
