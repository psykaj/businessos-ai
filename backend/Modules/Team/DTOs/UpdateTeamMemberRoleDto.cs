using System.ComponentModel.DataAnnotations;

namespace backend.Modules.Team.DTOs;

public class UpdateTeamMemberRoleDto
{
    [Required]
    public Guid RoleId { get; set; }
}
