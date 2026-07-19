using backend.Entities;
using backend.Interfaces;

namespace backend.Modules.Team.Repositories;

public interface IInvitationRepository : IGenericRepository<Invitation>
{
    Task<Invitation?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<Invitation?> GetPendingByEmailAsync(Guid organizationId, string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Invitation>> GetOrganizationInvitationsAsync(Guid organizationId, CancellationToken cancellationToken = default);
}
