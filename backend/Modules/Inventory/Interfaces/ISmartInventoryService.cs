using backend.Modules.Inventory.DTOs;

namespace backend.Modules.Inventory.Interfaces;

public interface ISmartInventoryService
{
    Task<IEnumerable<StockAlertDto>> GetStockAlertsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReorderSuggestionDto>> GetReorderSuggestionsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductVelocityReportDto>> GetProductVelocityReportAsync(Guid organizationId, int days = 30, CancellationToken cancellationToken = default);
    Task<DashboardInventorySummaryDto> GetDashboardSummaryAsync(Guid organizationId, CancellationToken cancellationToken = default);
}
