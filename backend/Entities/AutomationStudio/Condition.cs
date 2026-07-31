using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Entities;

public class Condition : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid WorkflowId { get; set; }

    public Guid? ParentConditionId { get; set; } // For nested conditions (AND/OR logic)

    [Required]
    [MaxLength(100)]
    public string Type { get; set; } = string.Empty; // e.g., IfElse, Equals, GreaterThan

    [Column(TypeName = "jsonb")]
    public string Configuration { get; set; } = "{}";

    public int Order { get; set; }

    // Navigation properties
    public virtual Organization Organization { get; set; } = null!;
    public virtual AutomationWorkflow Workflow { get; set; } = null!;
    public virtual Condition? ParentCondition { get; set; }
    public virtual ICollection<Condition> ChildConditions { get; set; } = new List<Condition>();
}
