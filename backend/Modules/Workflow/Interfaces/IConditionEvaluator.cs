using backend.Modules.Workflow.Entities;

namespace backend.Modules.Workflow.Interfaces;

public interface IConditionEvaluator
{
    bool EvaluateConditions(IEnumerable<WorkflowCondition> conditions, Dictionary<string, string> context);
    bool EvaluateCondition(WorkflowCondition condition, Dictionary<string, string> context);
}
