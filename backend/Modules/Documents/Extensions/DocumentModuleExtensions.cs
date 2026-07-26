using backend.Modules.Documents.Approvals.Interfaces;
using backend.Modules.Documents.Approvals.Repositories;
using backend.Modules.Documents.Approvals.Services;
using backend.Modules.Documents.AuditLogs.Interfaces;
using backend.Modules.Documents.AuditLogs.Repositories;
using backend.Modules.Documents.AuditLogs.Services;
using backend.Modules.Documents.Documents.Interfaces;
using backend.Modules.Documents.Documents.Repositories;
using backend.Modules.Documents.Documents.Services;
using backend.Modules.Documents.DocumentTemplates.Interfaces;
using backend.Modules.Documents.DocumentTemplates.Repositories;
using backend.Modules.Documents.DocumentTemplates.Services;
using backend.Modules.Documents.ESignatures.Interfaces;
using backend.Modules.Documents.ESignatures.Repositories;
using backend.Modules.Documents.ESignatures.Services;
using backend.Modules.Documents.Folders.Interfaces;
using backend.Modules.Documents.Folders.Repositories;
using backend.Modules.Documents.Folders.Services;
using backend.Modules.Documents.Sharing.Interfaces;
using backend.Modules.Documents.Sharing.Repositories;
using backend.Modules.Documents.Sharing.Services;
using backend.Modules.Documents.Storage.Interfaces;
using backend.Modules.Documents.Storage.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Modules.Documents.Extensions;

public static class DocumentModuleExtensions
{
    public static IServiceCollection AddDocumentModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Storage Services
        services.AddSingleton<LocalStorageService>();
        services.AddSingleton<AzureBlobStorageService>();
        services.AddScoped<IStorageService>(sp =>
        {
            var provider = configuration["Storage:Provider"] ?? "LocalStorage";
            return provider.Equals("AzureBlobStorage", StringComparison.OrdinalIgnoreCase)
                ? sp.GetRequiredService<AzureBlobStorageService>()
                : sp.GetRequiredService<LocalStorageService>();
        });

        // Audit Logs Sub-Module
        services.AddScoped<IDocumentAuditLogRepository, DocumentAuditLogRepository>();
        services.AddScoped<IDocumentAuditLogService, DocumentAuditLogService>();

        // Folders Sub-Module
        services.AddScoped<IFolderRepository, FolderRepository>();
        services.AddScoped<IFolderService, FolderService>();

        // Documents Sub-Module
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IDocumentService, DocumentService>();

        // Document Templates Sub-Module
        services.AddScoped<IDocumentTemplateRepository, DocumentTemplateRepository>();
        services.AddScoped<IDocumentTemplateService, DocumentTemplateService>();

        // Approvals Sub-Module
        services.AddScoped<IApprovalRepository, ApprovalRepository>();
        services.AddScoped<IApprovalService, ApprovalService>();

        // E-Signatures Sub-Module
        services.AddScoped<IESignatureRepository, ESignatureRepository>();
        services.AddScoped<IESignatureProviderService, NativeESignatureProviderService>();
        services.AddScoped<IESignatureService, ESignatureService>();

        // Sharing Sub-Module
        services.AddScoped<ISharedDocumentRepository, SharedDocumentRepository>();
        services.AddScoped<ISharedDocumentService, SharedDocumentService>();

        return services;
    }
}
