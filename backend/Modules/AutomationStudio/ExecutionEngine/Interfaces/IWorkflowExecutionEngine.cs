using System;
using System.Threading.Tasks;

namespace backend.Modules.AutomationStudio.ExecutionEngine.Interfaces;

public interface IWorkflowExecutionEngine
{
    Task ExecuteWorkflowAsync(Guid workflowId, string contextData);
}
