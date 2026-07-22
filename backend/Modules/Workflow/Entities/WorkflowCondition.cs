using backend.Common;
using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Entities;

public class WorkflowCondition : BaseEntity
{
    public Guid WorkflowId { get; set; }
    public Guid? WorkflowActionId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public ConditionOperator Operator { get; set; }
    public string Value { get; set; } = string.Empty;
    public LogicalOperator LogicalOperator { get; set; } = LogicalOperator.AND;

    // Navigation properties
    public Workflow Workflow { get; set; } = null!;
    public WorkflowAction? WorkflowAction { get; set; }
}
