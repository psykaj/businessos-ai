using backend.Modules.CRM.Interfaces;
using backend.Modules.CRM.Repositories;
using backend.Modules.CRM.Services;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Modules.CRM.Extensions;

public static class CrmExtensions
{
    public static IServiceCollection AddCrmModule(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<ILeadRepository, LeadRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IDealRepository, DealRepository>();
        services.AddScoped<ICrmActivityRepository, CrmActivityRepository>();
        services.AddScoped<ICrmTaskRepository, CrmTaskRepository>();
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IDealStageHistoryRepository, DealStageHistoryRepository>();

        // Services
        services.AddScoped<ILeadService, LeadService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IDealService, DealService>();
        services.AddScoped<ICrmActivityService, CrmActivityService>();
        services.AddScoped<ICrmTaskService, CrmTaskService>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<ITagService, TagService>();
        
        services.AddScoped<ICrmSearchService, CrmSearchService>();
        services.AddScoped<ICrmReportingService, CrmReportingService>();

        return services;
    }
}
