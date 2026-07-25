using backend.Modules.CustomerSuccess.Loyalty.Entities;

namespace backend.Modules.CustomerSuccess.Loyalty.Interfaces;

public interface ILoyaltyRepository
{
    Task<LoyaltyProgram?> GetProgramByIdAsync(Guid orgId, Guid id);
    Task<LoyaltyProgram?> GetDefaultProgramAsync(Guid orgId);
    Task<IEnumerable<LoyaltyProgram>> GetProgramsAsync(Guid orgId);
    Task AddProgramAsync(LoyaltyProgram program);
    Task UpdateProgramAsync(LoyaltyProgram program);

    Task<int> GetCustomerBalanceAsync(Guid orgId, Guid customerId, Guid programId);
    Task AddTransactionAsync(LoyaltyTransaction transaction);
    Task<(IEnumerable<LoyaltyTransaction> Items, int TotalCount)> GetTransactionsPagedAsync(
        Guid orgId, Guid? customerId, Guid? programId, int page, int pageSize);
    Task<IEnumerable<LoyaltyTransaction>> GetExpiredPointsAsync(Guid orgId, DateTime cutoffDate);
    Task SaveChangesAsync();
}
