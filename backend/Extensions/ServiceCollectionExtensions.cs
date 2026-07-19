using backend.Interfaces;
using backend.Persistence;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        
        // Register core module repositories
        services.AddScoped<backend.Modules.Organization.Repositories.IOrganizationRepository, backend.Modules.Organization.Repositories.OrganizationRepository>();
        services.AddScoped<backend.Modules.Team.Repositories.ITeamRepository, backend.Modules.Team.Repositories.TeamRepository>();
        services.AddScoped<backend.Modules.Team.Repositories.IInvitationRepository, backend.Modules.Team.Repositories.InvitationRepository>();
        services.AddScoped<backend.Modules.Roles.Repositories.IRoleRepository, backend.Modules.Roles.Repositories.RoleRepository>();
        services.AddScoped<backend.Modules.Permissions.Repositories.IPermissionRepository, backend.Modules.Permissions.Repositories.PermissionRepository>();
        services.AddScoped<backend.Modules.ApiKeys.Repositories.IApiKeyRepository, backend.Modules.ApiKeys.Repositories.ApiKeyRepository>();
        services.AddScoped<backend.Modules.AuditLogs.Repositories.IAuditLogRepository, backend.Modules.AuditLogs.Repositories.AuditLogRepository>();

        return services;
    }
}
