using backend.Modules.Team.DTOs;

namespace backend.Modules.Team.Interfaces;

public interface ITeamService
{
    Task<string> InviteTeamMemberAsync(Guid organizationId, InviteTeamMemberDto dto, CancellationToken cancellationToken = default);
    Task AcceptInvitationAsync(string token, Guid userId, CancellationToken cancellationToken = default);
    Task RejectInvitationAsync(string token, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TeamMemberDto>> ListTeamMembersAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<TeamMemberDto> UpdateMemberRoleAsync(Guid organizationId, Guid memberId, UpdateTeamMemberRoleDto dto, CancellationToken cancellationToken = default);
    Task RemoveMemberAsync(Guid organizationId, Guid memberId, CancellationToken cancellationToken = default);
    Task<TeamMemberDto> DeactivateMemberAsync(Guid organizationId, Guid memberId, CancellationToken cancellationToken = default);
    Task<TeamMemberDto> ReactivateMemberAsync(Guid organizationId, Guid memberId, CancellationToken cancellationToken = default);
}
