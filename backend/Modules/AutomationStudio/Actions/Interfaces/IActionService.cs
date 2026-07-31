using System;
using System.Threading.Tasks;

namespace backend.Modules.AutomationStudio.Actions.Interfaces;

public interface IActionService
{
    Task<bool> ExecuteActionAsync(backend.Entities.Action action, string contextData);
}
