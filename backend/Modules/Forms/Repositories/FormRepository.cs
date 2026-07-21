using backend.Modules.Forms.Entities;
using backend.Modules.Forms.Interfaces;
using backend.Persistence;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Forms.Repositories;

public class FormRepository : GenericRepository<Form>, IFormRepository
{
    private readonly ApplicationDbContext _context;

    public FormRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Form> Items, int TotalCount)> GetFormsPagedAsync(Guid organizationId, int pageNumber, int pageSize, string? search, string? status, CancellationToken cancellationToken = default)
    {
        var query = _context.Forms.Where(f => f.OrganizationId == organizationId && !f.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(f => f.Name.Contains(search) || f.Description.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(f => f.Status == status);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(f => f.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Form?> GetFormBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Forms
            .Include(f => f.Fields.OrderBy(field => field.Order))
            .FirstOrDefaultAsync(f => f.Slug == slug && f.Status == "Published" && !f.IsDeleted, cancellationToken);
    }

    public async Task<Form?> GetFormWithFieldsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Forms
            .Include(f => f.Fields.OrderBy(field => field.Order))
            .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted, cancellationToken);
    }
}
