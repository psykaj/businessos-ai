using backend.Entities;
using backend.Modules.Automation.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Automation.Repositories;

public class AutomationRepository : IAutomationRepository
{
    private readonly ApplicationDbContext _context;

    public AutomationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AutomationRule?> GetRuleAsync(Guid organizationId, Guid ruleId)
    {
        return await _context.AutomationRules
            .FirstOrDefaultAsync(r => r.OrganizationId == organizationId && r.Id == ruleId && !r.IsDeleted);
    }

    public async Task<IEnumerable<AutomationRule>> GetRulesByTriggerAsync(Guid organizationId, string trigger)
    {
        return await _context.AutomationRules
            .Where(r => r.OrganizationId == organizationId && r.Trigger == trigger && r.IsEnabled && !r.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<AutomationRule>> GetAllRulesAsync(Guid organizationId)
    {
        return await _context.AutomationRules
            .Where(r => r.OrganizationId == organizationId && !r.IsDeleted)
            .OrderBy(r => r.Name)
            .ToListAsync();
    }

    public async Task<AutomationRule> AddRuleAsync(AutomationRule rule)
    {
        _context.AutomationRules.Add(rule);
        await _context.SaveChangesAsync();
        return rule;
    }

    public async Task UpdateRuleAsync(AutomationRule rule)
    {
        _context.AutomationRules.Update(rule);
        await _context.SaveChangesAsync();
    }

    public async Task<AutomationLog> AddLogAsync(AutomationLog log)
    {
        _context.AutomationLogs.Add(log);
        await _context.SaveChangesAsync();
        return log;
    }

    public async Task<IEnumerable<AutomationLog>> GetLogsAsync(Guid organizationId, Guid ruleId, int limit = 100)
    {
        return await _context.AutomationLogs
            .Where(l => l.OrganizationId == organizationId && l.RuleId == ruleId && !l.IsDeleted)
            .OrderByDescending(l => l.TriggeredAt)
            .Take(limit)
            .ToListAsync();
    }
}
