using System;
using System.Threading.Tasks;

namespace backend.Modules.AutomationStudio.Triggers.Interfaces;

public interface ITriggerService
{
    Task ProcessTriggerAsync(Guid organizationId, string triggerType, string payload);
}
