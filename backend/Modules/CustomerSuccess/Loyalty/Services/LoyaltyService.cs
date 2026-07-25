using backend.Modules.CustomerSuccess.Loyalty.DTOs;
using backend.Modules.CustomerSuccess.Loyalty.Entities;
using backend.Modules.CustomerSuccess.Loyalty.Interfaces;
using backend.Modules.CustomerSuccess.SuccessTasks.Entities;
using backend.Modules.CustomerSuccess.SuccessTasks.Interfaces;

namespace backend.Modules.CustomerSuccess.Loyalty.Services;

public class LoyaltyService : ILoyaltyService
{
    private readonly ILoyaltyRepository _loyaltyRepo;
    private readonly ISuccessTaskRepository _taskRepo;

    public LoyaltyService(ILoyaltyRepository loyaltyRepo, ISuccessTaskRepository taskRepo)
    {
        _loyaltyRepo = loyaltyRepo;
        _taskRepo = taskRepo;
    }

    public async Task<IEnumerable<LoyaltyProgramDto>> GetProgramsAsync(Guid orgId)
    {
        var programs = await _loyaltyRepo.GetProgramsAsync(orgId);
        return programs.Select(MapProgramToDto);
    }

    public async Task<LoyaltyProgramDto> GetProgramByIdAsync(Guid orgId, Guid id)
    {
        var program = await _loyaltyRepo.GetProgramByIdAsync(orgId, id)
            ?? throw new KeyNotFoundException("Loyalty program not found.");

        return MapProgramToDto(program);
    }

    public async Task<LoyaltyProgramDto> CreateProgramAsync(Guid orgId, CreateLoyaltyProgramDto dto)
    {
        var program = new LoyaltyProgram
        {
            OrganizationId = orgId,
            Name = dto.Name,
            Description = dto.Description,
            Status = "Active",
            PointsPerPurchase = Math.Max(1, dto.PointsPerPurchase),
            MinimumRedemptionPoints = Math.Max(1, dto.MinimumRedemptionPoints),
            PointsExpiryDays = dto.PointsExpiryDays,
            IsDefault = dto.IsDefault
        };

        if (dto.IsDefault)
        {
            var existingDefault = await _loyaltyRepo.GetDefaultProgramAsync(orgId);
            if (existingDefault != null)
            {
                existingDefault.IsDefault = false;
                await _loyaltyRepo.UpdateProgramAsync(existingDefault);
            }
        }

        await _loyaltyRepo.AddProgramAsync(program);
        await _loyaltyRepo.SaveChangesAsync();

        return MapProgramToDto(program);
    }

    public async Task<LoyaltyProgramDto> UpdateProgramAsync(Guid orgId, Guid id, UpdateLoyaltyProgramDto dto)
    {
        var program = await _loyaltyRepo.GetProgramByIdAsync(orgId, id)
            ?? throw new KeyNotFoundException("Loyalty program not found.");

        program.Name = dto.Name;
        program.Description = dto.Description;
        program.Status = dto.Status;
        program.PointsPerPurchase = Math.Max(1, dto.PointsPerPurchase);
        program.MinimumRedemptionPoints = Math.Max(1, dto.MinimumRedemptionPoints);
        program.PointsExpiryDays = dto.PointsExpiryDays;

        if (dto.IsDefault && !program.IsDefault)
        {
            var existingDefault = await _loyaltyRepo.GetDefaultProgramAsync(orgId);
            if (existingDefault != null && existingDefault.Id != id)
            {
                existingDefault.IsDefault = false;
                await _loyaltyRepo.UpdateProgramAsync(existingDefault);
            }
            program.IsDefault = true;
        }

        await _loyaltyRepo.UpdateProgramAsync(program);
        await _loyaltyRepo.SaveChangesAsync();

        return MapProgramToDto(program);
    }

    public async Task<LoyaltyTransactionDto> EarnPointsAsync(Guid orgId, EarnPointsDto dto)
    {
        var program = dto.ProgramId.HasValue 
            ? await _loyaltyRepo.GetProgramByIdAsync(orgId, dto.ProgramId.Value)
            : await _loyaltyRepo.GetDefaultProgramAsync(orgId);

        if (program == null)
        {
            throw new InvalidOperationException("No active loyalty program found.");
        }

        int pointsEarned = (int)Math.Floor(dto.PurchaseAmount * program.PointsPerPurchase);
        if (pointsEarned <= 0) pointsEarned = 1;

        int currentBalance = await _loyaltyRepo.GetCustomerBalanceAsync(orgId, dto.CustomerId, program.Id);
        int newBalance = currentBalance + pointsEarned;

        DateTime? expiryDate = program.PointsExpiryDays.HasValue 
            ? DateTime.UtcNow.AddDays(program.PointsExpiryDays.Value) 
            : null;

        var tx = new LoyaltyTransaction
        {
            OrganizationId = orgId,
            CustomerId = dto.CustomerId,
            ProgramId = program.Id,
            PointsEarned = pointsEarned,
            PointsRedeemed = 0,
            Balance = newBalance,
            TransactionType = "Earned",
            Description = dto.Description ?? $"Earned {pointsEarned} points for purchase of {dto.PurchaseAmount:C}",
            ExpiryDate = expiryDate
        };

        await _loyaltyRepo.AddTransactionAsync(tx);
        await _loyaltyRepo.SaveChangesAsync();

        // Milestone Alert Check (e.g. crossing 1000 points)
        if (currentBalance < 1000 && newBalance >= 1000)
        {
            await _taskRepo.AddAsync(new SuccessTask
            {
                OrganizationId = orgId,
                CustomerId = dto.CustomerId,
                TaskType = "CongratulateMilestone",
                Title = "Loyalty Milestone Reached (1,000 Points)",
                Description = $"Customer has reached {newBalance} loyalty points. Send a congratulations message or special reward.",
                Priority = "Medium",
                Status = "Pending",
                DueDate = DateTime.UtcNow.AddDays(2)
            });
            await _taskRepo.SaveChangesAsync();
        }

        return MapTransactionToDto(tx);
    }

    public async Task<LoyaltyTransactionDto> RedeemPointsAsync(Guid orgId, RedeemPointsDto dto)
    {
        var program = dto.ProgramId.HasValue
            ? await _loyaltyRepo.GetProgramByIdAsync(orgId, dto.ProgramId.Value)
            : await _loyaltyRepo.GetDefaultProgramAsync(orgId);

        if (program == null)
        {
            throw new InvalidOperationException("No active loyalty program found.");
        }

        int currentBalance = await _loyaltyRepo.GetCustomerBalanceAsync(orgId, dto.CustomerId, program.Id);

        if (dto.PointsToRedeem > currentBalance)
        {
            throw new InvalidOperationException($"Insufficient loyalty point balance. Current balance is {currentBalance}.");
        }

        if (dto.PointsToRedeem < program.MinimumRedemptionPoints)
        {
            throw new InvalidOperationException($"Minimum redemption threshold is {program.MinimumRedemptionPoints} points.");
        }

        int newBalance = currentBalance - dto.PointsToRedeem;

        var tx = new LoyaltyTransaction
        {
            OrganizationId = orgId,
            CustomerId = dto.CustomerId,
            ProgramId = program.Id,
            PointsEarned = 0,
            PointsRedeemed = dto.PointsToRedeem,
            Balance = newBalance,
            TransactionType = "Redeemed",
            Description = dto.Description ?? $"Redeemed {dto.PointsToRedeem} points"
        };

        await _loyaltyRepo.AddTransactionAsync(tx);
        await _loyaltyRepo.SaveChangesAsync();

        return MapTransactionToDto(tx);
    }

    public async Task<LoyaltyTransactionDto> AdjustPointsAsync(Guid orgId, AdjustPointsDto dto)
    {
        var program = dto.ProgramId.HasValue
            ? await _loyaltyRepo.GetProgramByIdAsync(orgId, dto.ProgramId.Value)
            : await _loyaltyRepo.GetDefaultProgramAsync(orgId);

        if (program == null)
        {
            throw new InvalidOperationException("No active loyalty program found.");
        }

        int currentBalance = await _loyaltyRepo.GetCustomerBalanceAsync(orgId, dto.CustomerId, program.Id);
        int newBalance = Math.Max(0, currentBalance + dto.PointsDelta);

        var tx = new LoyaltyTransaction
        {
            OrganizationId = orgId,
            CustomerId = dto.CustomerId,
            ProgramId = program.Id,
            PointsEarned = dto.PointsDelta > 0 ? dto.PointsDelta : 0,
            PointsRedeemed = dto.PointsDelta < 0 ? Math.Abs(dto.PointsDelta) : 0,
            Balance = newBalance,
            TransactionType = "Adjusted",
            Description = dto.Reason
        };

        await _loyaltyRepo.AddTransactionAsync(tx);
        await _loyaltyRepo.SaveChangesAsync();

        return MapTransactionToDto(tx);
    }

    public async Task<CustomerLoyaltySummaryDto> GetCustomerSummaryAsync(Guid orgId, Guid customerId, Guid? programId)
    {
        var program = programId.HasValue
            ? await _loyaltyRepo.GetProgramByIdAsync(orgId, programId.Value)
            : await _loyaltyRepo.GetDefaultProgramAsync(orgId);

        var (items, _) = await _loyaltyRepo.GetTransactionsPagedAsync(orgId, customerId, program?.Id, 1, 50);
        var txList = items.ToList();

        int balance = txList.FirstOrDefault()?.Balance ?? 0;
        int totalEarned = txList.Sum(t => t.PointsEarned);
        int totalRedeemed = txList.Sum(t => t.PointsRedeemed);

        return new CustomerLoyaltySummaryDto(
            customerId,
            balance,
            totalEarned,
            totalRedeemed,
            txList.Select(MapTransactionToDto)
        );
    }

    public async Task<(IEnumerable<LoyaltyTransactionDto> Items, int TotalCount)> GetTransactionsPagedAsync(
        Guid orgId, Guid? customerId, Guid? programId, int page, int pageSize)
    {
        var (items, totalCount) = await _loyaltyRepo.GetTransactionsPagedAsync(orgId, customerId, programId, page, pageSize);
        return (items.Select(MapTransactionToDto), totalCount);
    }

    private static LoyaltyProgramDto MapProgramToDto(LoyaltyProgram p)
    {
        return new LoyaltyProgramDto(
            p.Id,
            p.Name,
            p.Description,
            p.Status,
            p.PointsPerPurchase,
            p.MinimumRedemptionPoints,
            p.PointsExpiryDays,
            p.IsDefault,
            p.CreatedAt
        );
    }

    private static LoyaltyTransactionDto MapTransactionToDto(LoyaltyTransaction t)
    {
        return new LoyaltyTransactionDto(
            t.Id,
            t.CustomerId,
            t.Customer?.Name ?? "Customer",
            t.ProgramId,
            t.Program?.Name ?? "Default Program",
            t.PointsEarned,
            t.PointsRedeemed,
            t.Balance,
            t.TransactionType,
            t.Description,
            t.ExpiryDate,
            t.CreatedAt
        );
    }
}
