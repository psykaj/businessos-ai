using backend.Common;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Inventory.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly ApplicationDbContext _context;

    public SupplierRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Supplier?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Suppliers
            .Include(s => s.PurchaseOrders)
            .FirstOrDefaultAsync(s => s.Id == id && s.OrganizationId == organizationId && !s.IsDeleted, cancellationToken);
    }

    public async Task<Supplier?> GetByCodeAsync(string code, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Suppliers
            .FirstOrDefaultAsync(s => s.Code.ToLower() == code.ToLower() && s.OrganizationId == organizationId && !s.IsDeleted, cancellationToken);
    }

    public async Task<PagedResult<Supplier>> GetPagedAsync(Guid organizationId, string? query, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var dbQuery = _context.Suppliers
            .Where(s => s.OrganizationId == organizationId && !s.IsDeleted);

        if (!string.IsNullOrWhiteSpace(query))
        {
            var searchTerm = query.Trim().ToLower();
            dbQuery = dbQuery.Where(s => s.Name.ToLower().Contains(searchTerm) ||
                                         s.Code.ToLower().Contains(searchTerm) ||
                                         s.ContactPerson.ToLower().Contains(searchTerm) ||
                                         s.Email.ToLower().Contains(searchTerm));
        }

        var totalCount = await dbQuery.CountAsync(cancellationToken);

        var items = await dbQuery
            .OrderBy(s => s.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Supplier>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Suppliers
            .Where(s => s.OrganizationId == organizationId && !s.IsDeleted && s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default)
    {
        await _context.Suppliers.AddAsync(supplier, cancellationToken);
    }

    public void Update(Supplier supplier)
    {
        _context.Suppliers.Update(supplier);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
