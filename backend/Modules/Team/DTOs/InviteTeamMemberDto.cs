using System.ComponentModel.DataAnnotations;

namespace backend.Modules.Team.DTOs;

public class InviteTeamMemberDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public Guid RoleId { get; set; }
}
