using backend.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Team.Repositories;

public class TeamRepository : backend.Repositories.GenericRepository<TeamMember>, ITeamRepository
{
    private readonly ApplicationDbContext _context;

    public TeamRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<TeamMember?> GetTeamMemberAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.TeamMembers
            .Include(t => t.Role)
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.OrganizationId == organizationId && t.UserId == userId && !t.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<TeamMember>> GetTeamMembersAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.TeamMembers
            .Include(t => t.Role)
            .Include(t => t.User)
            .Where(t => t.OrganizationId == organizationId && !t.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<TeamMember> Items, int TotalCount)> GetPagedTeamMembersAsync(
        Guid organizationId, int page, int pageSize, string? searchTerm, CancellationToken cancellationToken = default)
    {
        var query = _context.TeamMembers
            .Include(t => t.Role)
            .Include(t => t.User)
            .Where(t => t.OrganizationId == organizationId && !t.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t => t.User!.FullName.Contains(searchTerm) || t.User!.Email.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .OrderByDescending(t => t.JoinedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
