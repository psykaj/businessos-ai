namespace backend.Modules.AiAgent.CommandEngine.Models;

public class CommandParseResult
{
    public string ToolName { get; set; } = string.Empty;
    public string Intent { get; set; } = string.Empty;
    public Dictionary<string, object> Parameters { get; set; } = new();
    public double Confidence { get; set; } = 1.0;
    public string Explanation { get; set; } = string.Empty;
}
