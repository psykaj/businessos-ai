using backend.Entities;
using backend.Exceptions;
using backend.Interfaces;
using backend.Modules.Team.DTOs;
using backend.Modules.Team.Interfaces;
using backend.Modules.Team.Repositories;

namespace backend.Modules.Team.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TeamService(ITeamRepository teamRepository, IInvitationRepository invitationRepository, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> InviteTeamMemberAsync(Guid organizationId, InviteTeamMemberDto dto, CancellationToken cancellationToken = default)
    {
        // 1. Check if invitation already exists and is pending
        var existingInvite = await _invitationRepository.GetPendingByEmailAsync(organizationId, dto.Email, cancellationToken);
        if (existingInvite != null)
            throw new BadRequestException("An invitation to this email is already pending.");

        // 2. Generate secure token
        var token = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32));

        // 3. Create invitation
        var invitation = new Invitation
        {
            Email = dto.Email,
            Token = token,
            OrganizationId = organizationId,
            RoleId = dto.RoleId,
            ExpiresAt = DateTime.UtcNow.AddDays(7), // Token expires in 7 days
            Status = "Pending"
        };

        await _invitationRepository.AddAsync(invitation, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Here we would typically send an email. For now we return the token.
        return token;
    }

    public async Task AcceptInvitationAsync(string token, Guid userId, CancellationToken cancellationToken = default)
    {
        var invitation = await _invitationRepository.GetByTokenAsync(token, cancellationToken);
        if (invitation == null || invitation.Status != "Pending" || invitation.ExpiresAt < DateTime.UtcNow)
            throw new BadRequestException("Invalid or expired invitation token.");

        // Add user to team
        var teamMember = new TeamMember
        {
            OrganizationId = invitation.OrganizationId,
            UserId = userId,
            RoleId = invitation.RoleId,
            Status = "Active",
            JoinedAt = DateTime.UtcNow
        };

        await _teamRepository.AddAsync(teamMember, cancellationToken);
        
        invitation.Status = "Accepted";
        _invitationRepository.Update(invitation);

        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task RejectInvitationAsync(string token, CancellationToken cancellationToken = default)
    {
        var invitation = await _invitationRepository.GetByTokenAsync(token, cancellationToken);
        if (invitation == null || invitation.Status != "Pending")
            throw new BadRequestException("Invalid invitation token.");

        invitation.Status = "Rejected";
        _invitationRepository.Update(invitation);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TeamMemberDto>> ListTeamMembersAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var members = await _teamRepository.GetTeamMembersAsync(organizationId, cancellationToken);
        return members.Select(m => new TeamMemberDto
        {
            Id = m.Id,
            OrganizationId = m.OrganizationId,
            UserId = m.UserId,
            UserFullName = m.User?.FullName ?? string.Empty,
            UserEmail = m.User?.Email ?? string.Empty,
            RoleId = m.RoleId,
            RoleName = m.Role?.Name ?? string.Empty,
            Status = m.Status,
            LastLogin = m.LastLogin,
            JoinedAt = m.JoinedAt
        }).ToList();
    }

    public async Task<TeamMemberDto> UpdateMemberRoleAsync(Guid organizationId, Guid memberId, UpdateTeamMemberRoleDto dto, CancellationToken cancellationToken = default)
    {
        var member = await _teamRepository.GetByIdAsync(memberId, cancellationToken);
        if (member == null || member.OrganizationId != organizationId)
            throw new NotFoundException("Team member not found.");

        member.RoleId = dto.RoleId;
        _teamRepository.Update(member);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Fetch again to get updated role details
        return (await ListTeamMembersAsync(organizationId, cancellationToken)).First(m => m.Id == memberId);
    }

    public async Task RemoveMemberAsync(Guid organizationId, Guid memberId, CancellationToken cancellationToken = default)
    {
        var member = await _teamRepository.GetByIdAsync(memberId, cancellationToken);
        if (member == null || member.OrganizationId != organizationId)
            throw new NotFoundException("Team member not found.");

        _teamRepository.Delete(member);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<TeamMemberDto> DeactivateMemberAsync(Guid organizationId, Guid memberId, CancellationToken cancellationToken = default)
    {
        var member = await _teamRepository.GetByIdAsync(memberId, cancellationToken);
        if (member == null || member.OrganizationId != organizationId)
            throw new NotFoundException("Team member not found.");

        member.Status = "Inactive";
        _teamRepository.Update(member);
        await _unitOfWork.CompleteAsync(cancellationToken);
        
        return (await ListTeamMembersAsync(organizationId, cancellationToken)).First(m => m.Id == memberId);
    }

    public async Task<TeamMemberDto> ReactivateMemberAsync(Guid organizationId, Guid memberId, CancellationToken cancellationToken = default)
    {
        var member = await _teamRepository.GetByIdAsync(memberId, cancellationToken);
        if (member == null || member.OrganizationId != organizationId)
            throw new NotFoundException("Team member not found.");

        member.Status = "Active";
        _teamRepository.Update(member);
        await _unitOfWork.CompleteAsync(cancellationToken);
        
        return (await ListTeamMembersAsync(organizationId, cancellationToken)).First(m => m.Id == memberId);
    }
}
