using backend.Modules.Accounting.DTOs;
using backend.Modules.Accounting.Entities;
using backend.Modules.Accounting.Interfaces;

namespace backend.Modules.Accounting.Services;

public class AccountingService : IAccountingService
{
    private readonly IAccountRepository _repository;

    public AccountingService(IAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<AccountDto> CreateAccountAsync(Guid organizationId, CreateAccountDto dto)
    {
        var existing = await _repository.GetByCodeAsync(dto.AccountCode, organizationId);
        if (existing != null)
        {
            throw new InvalidOperationException($"Account code '{dto.AccountCode}' already exists for this organization.");
        }

        var account = new Account
        {
            OrganizationId = organizationId,
            AccountCode = dto.AccountCode,
            AccountName = dto.AccountName,
            AccountType = dto.AccountType,
            SubCategory = dto.SubCategory,
            Currency = dto.Currency,
            CurrentBalance = dto.InitialBalance,
            Description = dto.Description,
            IsActive = true
        };

        var created = await _repository.AddAsync(account);
        return MapToDto(created);
    }

    public async Task<AccountDto?> UpdateAccountAsync(Guid id, Guid organizationId, UpdateAccountDto dto)
    {
        var account = await _repository.GetByIdAsync(id, organizationId);
        if (account == null) return null;

        account.AccountName = dto.AccountName;
        account.AccountType = dto.AccountType;
        account.SubCategory = dto.SubCategory;
        account.Currency = dto.Currency;
        account.IsActive = dto.IsActive;
        account.Description = dto.Description;

        await _repository.UpdateAsync(account);
        return MapToDto(account);
    }

    public async Task<bool> DeleteAccountAsync(Guid id, Guid organizationId)
    {
        var account = await _repository.GetByIdAsync(id, organizationId);
        if (account == null) return false;

        await _repository.DeleteAsync(account);
        return true;
    }

    public async Task<AccountDto?> GetAccountByIdAsync(Guid id, Guid organizationId)
    {
        var account = await _repository.GetByIdAsync(id, organizationId);
        return account != null ? MapToDto(account) : null;
    }

    public async Task<IEnumerable<AccountDto>> GetChartOfAccountsAsync(Guid organizationId, string? accountType = null, bool activeOnly = false)
    {
        var accounts = await _repository.GetAllAsync(organizationId, accountType, activeOnly);
        if (!accounts.Any())
        {
            // Auto-seed default Chart of Accounts for new organization
            await SeedDefaultChartOfAccountsAsync(organizationId);
            accounts = await _repository.GetAllAsync(organizationId, accountType, activeOnly);
        }
        return accounts.Select(MapToDto);
    }

    public async Task SeedDefaultChartOfAccountsAsync(Guid organizationId)
    {
        var defaults = new List<Account>
        {
            new Account { OrganizationId = organizationId, AccountCode = "1010", AccountName = "Cash on Hand", AccountType = "Asset", SubCategory = "Current Asset", CurrentBalance = 0 },
            new Account { OrganizationId = organizationId, AccountCode = "1020", AccountName = "Operating Bank Account", AccountType = "Asset", SubCategory = "Current Asset", CurrentBalance = 0 },
            new Account { OrganizationId = organizationId, AccountCode = "1100", AccountName = "Accounts Receivable", AccountType = "Asset", SubCategory = "Current Asset", CurrentBalance = 0 },
            new Account { OrganizationId = organizationId, AccountCode = "2000", AccountName = "Accounts Payable", AccountType = "Liability", SubCategory = "Current Liability", CurrentBalance = 0 },
            new Account { OrganizationId = organizationId, AccountCode = "2100", AccountName = "Sales Tax Payable", AccountType = "Liability", SubCategory = "Current Liability", CurrentBalance = 0 },
            new Account { OrganizationId = organizationId, AccountCode = "3000", AccountName = "Owner's Equity", AccountType = "Equity", SubCategory = "Equity", CurrentBalance = 0 },
            new Account { OrganizationId = organizationId, AccountCode = "4000", AccountName = "Sales Revenue", AccountType = "Revenue", SubCategory = "Operating Revenue", CurrentBalance = 0 },
            new Account { OrganizationId = organizationId, AccountCode = "5000", AccountName = "Cost of Goods Sold", AccountType = "Expense", SubCategory = "Direct Expense", CurrentBalance = 0 },
            new Account { OrganizationId = organizationId, AccountCode = "6010", AccountName = "Office Expenses", AccountType = "Expense", SubCategory = "Operating Expense", CurrentBalance = 0 },
            new Account { OrganizationId = organizationId, AccountCode = "6020", AccountName = "Rent & Utilities", AccountType = "Expense", SubCategory = "Operating Expense", CurrentBalance = 0 },
            new Account { OrganizationId = organizationId, AccountCode = "6030", AccountName = "Salaries & Wages", AccountType = "Expense", SubCategory = "Operating Expense", CurrentBalance = 0 },
            new Account { OrganizationId = organizationId, AccountCode = "6040", AccountName = "Software & Subscriptions", AccountType = "Expense", SubCategory = "Operating Expense", CurrentBalance = 0 }
        };

        foreach (var acc in defaults)
        {
            var existing = await _repository.GetByCodeAsync(acc.AccountCode, organizationId);
            if (existing == null)
            {
                await _repository.AddAsync(acc);
            }
        }
    }

    private static AccountDto MapToDto(Account a) => new(
        a.Id,
        a.OrganizationId,
        a.AccountCode,
        a.AccountName,
        a.AccountType,
        a.SubCategory,
        a.Currency,
        a.CurrentBalance,
        a.IsActive,
        a.Description,
        a.CreatedAt
    );
}
