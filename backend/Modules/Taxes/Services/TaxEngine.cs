using backend.Modules.Taxes.DTOs;
using backend.Modules.Taxes.Entities;
using backend.Modules.Taxes.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Taxes.Services;

public class TaxEngine : ITaxEngine
{
    private readonly ITaxRecordRepository _repository;
    private readonly ApplicationDbContext _context;

    public TaxEngine(ITaxRecordRepository repository, ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<TaxRecordDto>> GetTaxesAsync(Guid organizationId)
    {
        var taxes = await _repository.GetAllAsync(organizationId);
        if (!taxes.Any())
        {
            await SeedDefaultTaxesAsync(organizationId, "US");
            taxes = await _repository.GetAllAsync(organizationId);
        }
        return taxes.Select(MapToDto);
    }

    public async Task<TaxRecordDto> CreateTaxAsync(Guid organizationId, CreateTaxRecordDto dto)
    {
        var tax = new TaxRecord
        {
            OrganizationId = organizationId,
            TaxName = dto.TaxName,
            TaxCode = dto.TaxCode,
            Rate = dto.Rate,
            TaxType = dto.TaxType,
            CountryCode = dto.CountryCode,
            Description = dto.Description,
            IsActive = true
        };

        var created = await _repository.AddAsync(tax);
        return MapToDto(created);
    }

    public async Task<CalculateTaxResultDto> CalculateTaxAsync(Guid organizationId, CalculateTaxRequestDto request)
    {
        var tax = await _repository.GetByCodeAsync(request.TaxCode, organizationId);
        var rate = tax?.Rate ?? 0m;
        var taxName = tax?.TaxName ?? "Standard Tax";

        decimal baseAmount, taxAmount, totalAmount;

        if (request.IsInclusive)
        {
            totalAmount = request.Amount;
            baseAmount = Math.Round(totalAmount / (1 + (rate / 100m)), 2);
            taxAmount = totalAmount - baseAmount;
        }
        else
        {
            baseAmount = request.Amount;
            taxAmount = Math.Round(baseAmount * (rate / 100m), 2);
            totalAmount = baseAmount + taxAmount;
        }

        return new CalculateTaxResultDto(
            baseAmount,
            taxAmount,
            totalAmount,
            rate,
            request.TaxCode,
            taxName
        );
    }

    public async Task<TaxSummaryDto> GetTaxSummaryAsync(Guid organizationId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var start = startDate ?? DateTime.UtcNow.AddYears(-1);
        var end = endDate ?? DateTime.UtcNow;

        // Invoices tax collected
        var invoices = await _context.FinanceInvoices
            .AsNoTracking()
            .Where(i => i.OrganizationId == organizationId && i.IssueDate >= start && i.IssueDate <= end && !i.IsDeleted)
            .ToListAsync();

        var totalTaxCollected = invoices.Sum(i => i.TaxTotal);

        // Expenses tax paid
        var expenses = await _context.Expenses
            .AsNoTracking()
            .Where(e => e.OrganizationId == organizationId && e.ExpenseDate >= start && e.ExpenseDate <= end && !e.IsDeleted)
            .ToListAsync();

        var totalTaxPaid = expenses.Sum(e => e.TaxAmount);
        var netTaxLiability = totalTaxCollected - totalTaxPaid;

        var breakdowns = new List<TaxBreakdownDto>
        {
            new TaxBreakdownDto("GST18", "Standard GST (18%)", 18.0m, totalTaxCollected * 0.7m, totalTaxPaid * 0.7m),
            new TaxBreakdownDto("VAT5", "Reduced VAT (5%)", 5.0m, totalTaxCollected * 0.3m, totalTaxPaid * 0.3m)
        };

        return new TaxSummaryDto(
            totalTaxCollected,
            totalTaxPaid,
            netTaxLiability,
            breakdowns
        );
    }

    public async Task SeedDefaultTaxesAsync(Guid organizationId, string countryCode = "US")
    {
        var defaults = countryCode switch
        {
            "IN" => new List<TaxRecord>
            {
                new TaxRecord { OrganizationId = organizationId, TaxName = "GST 18%", TaxCode = "GST18", Rate = 18.0m, TaxType = "GST", CountryCode = "IN" },
                new TaxRecord { OrganizationId = organizationId, TaxName = "GST 12%", TaxCode = "GST12", Rate = 12.0m, TaxType = "GST", CountryCode = "IN" },
                new TaxRecord { OrganizationId = organizationId, TaxName = "GST 5%", TaxCode = "GST5", Rate = 5.0m, TaxType = "GST", CountryCode = "IN" }
            },
            _ => new List<TaxRecord>
            {
                new TaxRecord { OrganizationId = organizationId, TaxName = "Sales Tax 10%", TaxCode = "ST10", Rate = 10.0m, TaxType = "SalesTax", CountryCode = "US" },
                new TaxRecord { OrganizationId = organizationId, TaxName = "Standard VAT 20%", TaxCode = "VAT20", Rate = 20.0m, TaxType = "VAT", CountryCode = "UK" },
                new TaxRecord { OrganizationId = organizationId, TaxName = "Exempt (0%)", TaxCode = "ZERO", Rate = 0.0m, TaxType = "VAT", CountryCode = "US" }
            }
        };

        foreach (var tax in defaults)
        {
            var existing = await _repository.GetByCodeAsync(tax.TaxCode, organizationId);
            if (existing == null)
            {
                await _repository.AddAsync(tax);
            }
        }
    }

    private static TaxRecordDto MapToDto(TaxRecord t) => new(
        t.Id,
        t.OrganizationId,
        t.TaxName,
        t.TaxCode,
        t.Rate,
        t.TaxType,
        t.CountryCode,
        t.IsActive,
        t.Description,
        t.CreatedAt
    );
}
