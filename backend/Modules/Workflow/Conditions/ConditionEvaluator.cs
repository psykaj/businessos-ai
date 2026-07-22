using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.Entities;
using backend.Modules.Workflow.Interfaces;

namespace backend.Modules.Workflow.Conditions;

public class ConditionEvaluator : IConditionEvaluator
{
    private readonly IVariableResolver _variableResolver;

    public ConditionEvaluator(IVariableResolver variableResolver)
    {
        _variableResolver = variableResolver;
    }

    public bool EvaluateConditions(IEnumerable<WorkflowCondition> conditions, Dictionary<string, string> context)
    {
        var list = conditions.ToList();
        if (list.Count == 0) return true;

        bool result = true;
        for (int i = 0; i < list.Count; i++)
        {
            var cond = list[i];
            bool eval = EvaluateCondition(cond, context);

            if (i == 0)
            {
                result = eval;
            }
            else
            {
                if (cond.LogicalOperator == LogicalOperator.OR)
                {
                    result = result || eval;
                }
                else
                {
                    result = result && eval;
                }
            }
        }

        return result;
    }

    public bool EvaluateCondition(WorkflowCondition condition, Dictionary<string, string> context)
    {
        var fieldValue = ExtractValueFromContext(condition.FieldName, context);
        var expectedValue = _variableResolver.Resolve(condition.Value, context);

        return condition.Operator switch
        {
            ConditionOperator.Equals => string.Equals(fieldValue, expectedValue, StringComparison.OrdinalIgnoreCase),
            ConditionOperator.NotEquals => !string.Equals(fieldValue, expectedValue, StringComparison.OrdinalIgnoreCase),
            ConditionOperator.Contains => fieldValue.Contains(expectedValue, StringComparison.OrdinalIgnoreCase),
            ConditionOperator.DoesNotContain => !fieldValue.Contains(expectedValue, StringComparison.OrdinalIgnoreCase),
            ConditionOperator.GreaterThan => CompareNumbers(fieldValue, expectedValue) > 0,
            ConditionOperator.LessThan => CompareNumbers(fieldValue, expectedValue) < 0,
            ConditionOperator.GreaterThanOrEqual => CompareNumbers(fieldValue, expectedValue) >= 0,
            ConditionOperator.LessThanOrEqual => CompareNumbers(fieldValue, expectedValue) <= 0,
            ConditionOperator.DateBefore => CompareDates(fieldValue, expectedValue) < 0,
            ConditionOperator.DateAfter => CompareDates(fieldValue, expectedValue) > 0,
            ConditionOperator.DateSameDay => CompareDates(fieldValue, expectedValue) == 0,
            ConditionOperator.CustomerSegmentEquals => string.Equals(fieldValue, expectedValue, StringComparison.OrdinalIgnoreCase),
            ConditionOperator.LeadScoreGreaterThan => CompareNumbers(fieldValue, expectedValue) > 0,
            ConditionOperator.PipelineStageEquals => string.Equals(fieldValue, expectedValue, StringComparison.OrdinalIgnoreCase),
            _ => true
        };
    }

    private static string ExtractValueFromContext(string fieldName, Dictionary<string, string> context)
    {
        if (context.TryGetValue(fieldName, out var value) && value != null)
            return value;

        foreach (var kvp in context)
        {
            if (kvp.Key.Equals(fieldName, StringComparison.OrdinalIgnoreCase))
                return kvp.Value ?? string.Empty;
        }

        return string.Empty;
    }

    private static double CompareNumbers(string val1, string val2)
    {
        double.TryParse(val1, out double n1);
        double.TryParse(val2, out double n2);
        return n1.CompareTo(n2);
    }

    private static int CompareDates(string val1, string val2)
    {
        if (DateTime.TryParse(val1, out var d1) && DateTime.TryParse(val2, out var d2))
        {
            return d1.Date.CompareTo(d2.Date);
        }
        return 0;
    }
}
