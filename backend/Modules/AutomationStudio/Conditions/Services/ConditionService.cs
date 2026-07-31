using System;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Modules.AutomationStudio.Conditions.Interfaces;

namespace backend.Modules.AutomationStudio.Conditions.Services;

public class ConditionService : IConditionService
{
    public Task<bool> EvaluateConditionAsync(backend.Entities.Condition condition, string contextData)
    {
        // Example context data parsing
        // In a real application, contextData is JSON, e.g., {"InvoiceAmount": 500}
        // condition.Configuration is JSON, e.g., {"Field": "InvoiceAmount", "Value": 100}

        try
        {
            using var contextDoc = JsonDocument.Parse(contextData);
            using var configDoc = JsonDocument.Parse(condition.Configuration);

            var rootContext = contextDoc.RootElement;
            var rootConfig = configDoc.RootElement;

            if (rootConfig.TryGetProperty("Field", out var fieldElement) &&
                rootConfig.TryGetProperty("Value", out var expectedValueElement))
            {
                string field = fieldElement.GetString() ?? "";
                string expectedValue = expectedValueElement.ToString();

                if (rootContext.TryGetProperty(field, out var actualValueElement))
                {
                    string actualValue = actualValueElement.ToString();

                    return Task.FromResult(condition.Type switch
                    {
                        "Equals" => actualValue == expectedValue,
                        "NotEquals" => actualValue != expectedValue,
                        "GreaterThan" => double.TryParse(actualValue, out var a) && double.TryParse(expectedValue, out var e) && a > e,
                        "LessThan" => double.TryParse(actualValue, out var a2) && double.TryParse(expectedValue, out var e2) && a2 < e2,
                        _ => false
                    });
                }
            }
        }
        catch (Exception)
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(false);
    }
}
