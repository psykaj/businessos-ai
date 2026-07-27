using backend.Modules.Inventory.Interfaces;
using backend.Modules.Inventory.Repositories;
using backend.Modules.Inventory.Services;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Modules.Inventory.Extensions;

public static class InventoryServiceCollectionExtensions
{
    public static IServiceCollection AddInventoryModule(this IServiceCollection services)
    {
        // Cache service
        services.AddScoped<InventoryCacheService>();

        // Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped<IStockMovementRepository, StockMovementRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
        services.AddScoped<IGoodsReceiptRepository, GoodsReceiptRepository>();
        services.AddScoped<IStockAdjustmentRepository, StockAdjustmentRepository>();

        // Services
        services.AddScoped<ProductService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<WarehouseService>();
        services.AddScoped<StockMovementService>();
        services.AddScoped<PurchasingService>();
        services.AddScoped<SupplierService>();
        services.AddScoped<SmartInventoryService>();

        return services;
    }
}
