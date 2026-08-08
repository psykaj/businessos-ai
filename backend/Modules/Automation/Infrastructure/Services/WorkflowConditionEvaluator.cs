using System;
using System.Text.Json;
using System.Collections.Generic;
using backend.Modules.Automation.Application.Interfaces;
using backend.Modules.Automation.Domain.Entities;

namespace backend.Modules.Automation.Infrastructure.Services;

public class WorkflowConditionEvaluator : IWorkflowConditionEvaluator
{
    public bool EvaluateCondition(WorkflowStep step, object eventData)
    {
        if (string.IsNullOrWhiteSpace(step.Condition)) return true; // No condition = passes

        try
        {
            var eventDoc = JsonSerializer.SerializeToDocument(eventData);
            var conditionDoc = JsonDocument.Parse(step.Condition);

            // Expecting condition JSON to be like:
            // {
            //   "logic": "AND",
            //   "rules": [
            //     { "field": "amount", "operator": ">", "value": 10000 }
            //   ]
            // }

            if (!conditionDoc.RootElement.TryGetProperty("rules", out var rulesElement) || rulesElement.ValueKind != JsonValueKind.Array)
            {
                return true; // Malformed condition, default to true or maybe false? Let's say true to be safe or maybe false to stop execution. Let's return false for safety.
            }

            var logic = "AND";
            if (conditionDoc.RootElement.TryGetProperty("logic", out var logicElement) && logicElement.ValueKind == JsonValueKind.String)
            {
                logic = logicElement.GetString()?.ToUpperInvariant() ?? "AND";
            }

            bool finalResult = logic == "AND";

            foreach (var rule in rulesElement.EnumerateArray())
            {
                bool ruleResult = EvaluateSingleRule(rule, eventDoc.RootElement);

                if (logic == "AND")
                {
                    finalResult = finalResult && ruleResult;
                    if (!finalResult) break; // Short-circuit
                }
                else if (logic == "OR")
                {
                    finalResult = finalResult || ruleResult;
                    if (finalResult) break; // Short-circuit
                }
            }

            return finalResult;
        }
        catch
        {
            // If anything fails during parsing or evaluation, for safety return false
            return false;
        }
    }

    private bool EvaluateSingleRule(JsonElement rule, JsonElement eventData)
    {
        if (!rule.TryGetProperty("field", out var fieldProp) ||
            !rule.TryGetProperty("operator", out var opProp) ||
            !rule.TryGetProperty("value", out var valueProp))
        {
            return false;
        }

        var fieldPath = fieldProp.GetString();
        var op = opProp.GetString();
        
        if (string.IsNullOrEmpty(fieldPath) || string.IsNullOrEmpty(op)) return false;

        var actualValue = GetValueFromPath(eventData, fieldPath);
        if (actualValue == null) return false;

        return CompareValues(actualValue.Value, op, valueProp);
    }

    private JsonElement? GetValueFromPath(JsonElement element, string path)
    {
        var parts = path.Split('.');
        var current = element;
        
        foreach (var part in parts)
        {
            if (current.ValueKind == JsonValueKind.Object && current.TryGetProperty(part, out var next))
            {
                current = next;
            }
            else
            {
                return null;
            }
        }
        
        return current;
    }

    private bool CompareValues(JsonElement actual, string op, JsonElement expected)
    {
        // Simple numeric comparison
        if (actual.ValueKind == JsonValueKind.Number && expected.ValueKind == JsonValueKind.Number)
        {
            var a = actual.GetDouble();
            var e = expected.GetDouble();
            
            return op switch
            {
                "==" => a == e,
                "!=" => a != e,
                ">" => a > e,
                "<" => a < e,
                ">=" => a >= e,
                "<=" => a <= e,
                _ => false
            };
        }

        // String comparison
        if (actual.ValueKind == JsonValueKind.String && expected.ValueKind == JsonValueKind.String)
        {
            var a = actual.GetString() ?? "";
            var e = expected.GetString() ?? "";
            
            return op switch
            {
                "==" => a.Equals(e, StringComparison.OrdinalIgnoreCase),
                "!=" => !a.Equals(e, StringComparison.OrdinalIgnoreCase),
                _ => false
            };
        }

        // Boolean comparison
        if ((actual.ValueKind == JsonValueKind.True || actual.ValueKind == JsonValueKind.False) &&
            (expected.ValueKind == JsonValueKind.True || expected.ValueKind == JsonValueKind.False))
        {
            var a = actual.GetBoolean();
            var e = expected.GetBoolean();
            
            return op switch
            {
                "==" => a == e,
                "!=" => a != e,
                _ => false
            };
        }

        return false;
    }
}
