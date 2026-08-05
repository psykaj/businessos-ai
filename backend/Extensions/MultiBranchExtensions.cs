using Microsoft.Extensions.DependencyInjection;
using backend.Modules.Locations.Interfaces;
using backend.Modules.Locations.Repositories;
using backend.Modules.Locations.Services;
using backend.Modules.Branches.Interfaces;
using backend.Modules.Branches.Repositories;
using backend.Modules.Branches.Services;
using backend.Modules.BranchSettings.Interfaces;
using backend.Modules.BranchSettings.Services;
using backend.Modules.Warehouses.Interfaces;
using backend.Modules.Warehouses.Repositories;
using backend.Modules.Warehouses.Services;
using backend.Modules.Transfers.Interfaces;
using backend.Modules.Transfers.Repositories;
using backend.Modules.Transfers.Services;
using backend.Modules.RegionalReports.Interfaces;
using backend.Modules.RegionalReports.Repositories;
using backend.Modules.RegionalReports.Services;
using backend.Modules.BranchAnalytics.Interfaces;
using backend.Modules.BranchAnalytics.Repositories;
using backend.Modules.BranchAnalytics.Services;
using backend.Modules.BranchAnalytics.BackgroundJobs;

namespace backend.Extensions;

public static class MultiBranchExtensions
{
    public static IServiceCollection AddMultiBranchModule(this IServiceCollection services)
    {
        // ─── Module 1: Locations ──────────────────────────────────────────────────
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<ILocationService, LocationService>();

        // ─── Module 2: Branches & Branch Managers ─────────────────────────────────
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IBranchService, BranchService>();

        // ─── Module 3: Branch Settings & Operations ───────────────────────────────
        services.AddScoped<IBranchSettingsService, BranchSettingsService>();

        // ─── Module 4: Multi-Branch Warehouses ────────────────────────────────────
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IWarehouseService, WarehouseService>();

        // ─── Module 5: Inventory Transfers ────────────────────────────────────────
        services.AddScoped<ITransferRepository, TransferRepository>();
        services.AddScoped<ITransferService, TransferService>();

        // ─── Module 6: Regional Reports & Analytics ───────────────────────────────
        services.AddScoped<IRegionalReportRepository, RegionalReportRepository>();
        services.AddScoped<IRegionalReportService, RegionalReportService>();

        // ─── Module 7: Branch Performance Engine ──────────────────────────────────
        services.AddScoped<IBranchPerformanceRepository, BranchPerformanceRepository>();
        services.AddScoped<IBranchPerformanceEngineService, BranchPerformanceEngineService>();
        services.AddHostedService<BranchPerformanceWorkerJob>();

        return services;
    }
}
