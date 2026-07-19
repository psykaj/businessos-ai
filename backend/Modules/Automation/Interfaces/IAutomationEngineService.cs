namespace backend.Modules.Automation.Interfaces;

public interface IAutomationEngineService
{
    Task ExecuteTriggerAsync(Guid organizationId, string trigger, Dictionary<string, string> payload);
}
