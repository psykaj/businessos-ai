using backend.Modules.Forms.Entities;
using backend.Modules.Forms.Interfaces;
using backend.Persistence;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Forms.Repositories;

public class SubmissionRepository : GenericRepository<FormSubmission>, ISubmissionRepository
{
    private readonly ApplicationDbContext _context;

    public SubmissionRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<FormSubmission> Items, int TotalCount)> GetSubmissionsPagedAsync(Guid organizationId, Guid formId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.FormSubmissions
            .Where(s => s.OrganizationId == organizationId && s.FormId == formId && !s.IsDeleted);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(s => s.SubmittedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<int> GetSubmissionCountAsync(Guid organizationId, Guid formId, CancellationToken cancellationToken = default)
    {
        return await _context.FormSubmissions
            .CountAsync(s => s.OrganizationId == organizationId && s.FormId == formId && !s.IsDeleted, cancellationToken);
    }
}
