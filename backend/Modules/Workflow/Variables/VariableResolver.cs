using System.Text.RegularExpressions;
using backend.Modules.Workflow.Interfaces;

namespace backend.Modules.Workflow.Variables;

public class VariableResolver : IVariableResolver
{
    private static readonly Regex VariableRegex = new(@"\{\{\s*([a-zA-Z0-9_\.\-]+)\s*\}\}", RegexOptions.Compiled);

    public string Resolve(string template, Dictionary<string, string> context)
    {
        if (string.IsNullOrEmpty(template)) return string.Empty;

        return VariableRegex.Replace(template, match =>
        {
            var key = match.Groups[1].Value.Trim();

            // Direct key match
            if (context.TryGetValue(key, out var val) && val != null)
                return val;

            // Normalized key match (e.g. customer.name -> CustomerName, customer_name -> CustomerName)
            var normalizedKey = NormalizeKey(key);
            foreach (var kvp in context)
            {
                if (NormalizeKey(kvp.Key).Equals(normalizedKey, StringComparison.OrdinalIgnoreCase))
                    return kvp.Value ?? string.Empty;
            }

            // Built-in system variables
            if (key.Equals("today.date", StringComparison.OrdinalIgnoreCase) || key.Equals("today_date", StringComparison.OrdinalIgnoreCase))
                return DateTime.UtcNow.ToString("yyyy-MM-dd");

            if (key.Equals("current.time", StringComparison.OrdinalIgnoreCase) || key.Equals("current_time", StringComparison.OrdinalIgnoreCase))
                return DateTime.UtcNow.ToString("O");

            return match.Value; // keep intact if unresolved
        });
    }

    public Dictionary<string, string> ResolveDictionary(Dictionary<string, string> templateDict, Dictionary<string, string> context)
    {
        var result = new Dictionary<string, string>();
        foreach (var kvp in templateDict)
        {
            result[kvp.Key] = Resolve(kvp.Value, context);
        }
        return result;
    }

    private static string NormalizeKey(string key)
    {
        return key.Replace(".", "").Replace("_", "").Replace("-", "");
    }
}
