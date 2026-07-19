using backend.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Team.Repositories;

public class InvitationRepository : backend.Repositories.GenericRepository<Invitation>, IInvitationRepository
{
    private readonly ApplicationDbContext _context;

    public InvitationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Invitation?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.Invitations
            .Include(i => i.Role)
            .Include(i => i.Organization)
            .FirstOrDefaultAsync(i => i.Token == token && !i.IsDeleted, cancellationToken);
    }

    public async Task<Invitation?> GetPendingByEmailAsync(Guid organizationId, string email, CancellationToken cancellationToken = default)
    {
        return await _context.Invitations
            .FirstOrDefaultAsync(i => i.OrganizationId == organizationId 
                                   && i.Email == email 
                                   && i.Status == "Pending" 
                                   && i.ExpiresAt > DateTime.UtcNow
                                   && !i.IsDeleted, 
                                 cancellationToken);
    }

    public async Task<IReadOnlyList<Invitation>> GetOrganizationInvitationsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Invitations
            .Include(i => i.Role)
            .Where(i => i.OrganizationId == organizationId && !i.IsDeleted)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
