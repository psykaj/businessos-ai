using backend.Entities;

namespace backend.Modules.Automation.Interfaces;

public interface IAutomationRepository
{
    Task<AutomationRule?> GetRuleAsync(Guid organizationId, Guid ruleId);
    Task<IEnumerable<AutomationRule>> GetRulesByTriggerAsync(Guid organizationId, string trigger);
    Task<IEnumerable<AutomationRule>> GetAllRulesAsync(Guid organizationId);
    Task<AutomationRule> AddRuleAsync(AutomationRule rule);
    Task UpdateRuleAsync(AutomationRule rule);
    
    Task<AutomationLog> AddLogAsync(AutomationLog log);
    Task<IEnumerable<AutomationLog>> GetLogsAsync(Guid organizationId, Guid ruleId, int limit = 100);
}
