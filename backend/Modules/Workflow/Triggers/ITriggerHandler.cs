using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Triggers;

public interface ITriggerHandler
{
    TriggerType TriggerType { get; }
    Task<Dictionary<string, string>> ExtractContextAsync(string triggerConfiguration, Dictionary<string, string> rawPayload);
}
