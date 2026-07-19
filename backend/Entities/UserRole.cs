namespace backend.Entities;

// Note: This is an intersection table entity for global roles.
// It doesn't need to inherit from BaseEntity if we configure it with composite keys,
// but for consistency we can inherit or just define it.
public class UserRole
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
}
