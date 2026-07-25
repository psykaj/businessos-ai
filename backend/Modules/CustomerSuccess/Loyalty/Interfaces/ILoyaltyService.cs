using backend.Modules.CustomerSuccess.Loyalty.DTOs;

namespace backend.Modules.CustomerSuccess.Loyalty.Interfaces;

public interface ILoyaltyService
{
    Task<IEnumerable<LoyaltyProgramDto>> GetProgramsAsync(Guid orgId);
    Task<LoyaltyProgramDto> GetProgramByIdAsync(Guid orgId, Guid id);
    Task<LoyaltyProgramDto> CreateProgramAsync(Guid orgId, CreateLoyaltyProgramDto dto);
    Task<LoyaltyProgramDto> UpdateProgramAsync(Guid orgId, Guid id, UpdateLoyaltyProgramDto dto);

    Task<LoyaltyTransactionDto> EarnPointsAsync(Guid orgId, EarnPointsDto dto);
    Task<LoyaltyTransactionDto> RedeemPointsAsync(Guid orgId, RedeemPointsDto dto);
    Task<LoyaltyTransactionDto> AdjustPointsAsync(Guid orgId, AdjustPointsDto dto);
    Task<CustomerLoyaltySummaryDto> GetCustomerSummaryAsync(Guid orgId, Guid customerId, Guid? programId);

    Task<(IEnumerable<LoyaltyTransactionDto> Items, int TotalCount)> GetTransactionsPagedAsync(
        Guid orgId, Guid? customerId, Guid? programId, int page, int pageSize);
}
