using System;
using System.Threading.Tasks;
using backend.Entities;

namespace backend.Modules.AutomationStudio.Conditions.Interfaces;

public interface IConditionService
{
    Task<bool> EvaluateConditionAsync(backend.Entities.Condition condition, string contextData);
}
