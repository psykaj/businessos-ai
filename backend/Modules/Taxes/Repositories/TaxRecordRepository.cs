using backend.Modules.Taxes.Entities;
using backend.Modules.Taxes.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Taxes.Repositories;

public class TaxRecordRepository : ITaxRecordRepository
{
    private readonly ApplicationDbContext _context;

    public TaxRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaxRecord?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.TaxRecords
            .FirstOrDefaultAsync(t => t.Id == id && t.OrganizationId == organizationId && !t.IsDeleted);
    }

    public async Task<TaxRecord?> GetByCodeAsync(string taxCode, Guid organizationId)
    {
        return await _context.TaxRecords
            .FirstOrDefaultAsync(t => t.TaxCode == taxCode && t.OrganizationId == organizationId && !t.IsDeleted);
    }

    public async Task<IEnumerable<TaxRecord>> GetAllAsync(Guid organizationId)
    {
        return await _context.TaxRecords
            .AsNoTracking()
            .Where(t => t.OrganizationId == organizationId && !t.IsDeleted)
            .OrderBy(t => t.TaxName)
            .ToListAsync();
    }

    public async Task<TaxRecord> AddAsync(TaxRecord tax)
    {
        await _context.TaxRecords.AddAsync(tax);
        await _context.SaveChangesAsync();
        return tax;
    }

    public async Task UpdateAsync(TaxRecord tax)
    {
        _context.TaxRecords.Update(tax);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TaxRecord tax)
    {
        tax.IsDeleted = true;
        tax.DeletedAt = DateTime.UtcNow;
        _context.TaxRecords.Update(tax);
        await _context.SaveChangesAsync();
    }
}
