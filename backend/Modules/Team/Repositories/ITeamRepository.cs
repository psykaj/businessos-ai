using backend.Entities;
using backend.Interfaces;

namespace backend.Modules.Team.Repositories;

public interface ITeamRepository : IGenericRepository<TeamMember>
{
    Task<TeamMember?> GetTeamMemberAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TeamMember>> GetTeamMembersAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<TeamMember> Items, int TotalCount)> GetPagedTeamMembersAsync(
        Guid organizationId, int page, int pageSize, string? searchTerm, CancellationToken cancellationToken = default);
}
