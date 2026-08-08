using System;
using System.Threading.Tasks;
using backend.Modules.Automation.Domain.Enums;
using backend.Modules.Automation.Domain.Entities;

namespace backend.Modules.Automation.Application.Interfaces;

public interface IWorkflowTriggerService
{
    Task TriggerEventAsync(WorkflowTriggerType triggerType, Guid organizationId, object eventData);
}

public interface IWorkflowEngine
{
    Task ExecuteWorkflowAsync(AiWorkflow workflow, Guid organizationId, object eventData);
}

public interface IWorkflowActionExecutor
{
    Task<bool> ExecuteActionAsync(WorkflowStep step, Guid organizationId, object eventData);
}

public interface IWorkflowConditionEvaluator
{
    bool EvaluateCondition(WorkflowStep step, object eventData);
}

public interface IAIDecisionService
{
    Task<AIDecisionResult> MakeDecisionAsync(WorkflowStep step, Guid organizationId, object eventData);
}

public interface IWorkflowApprovalService
{
    Task RequestApprovalAsync(WorkflowStep step, Guid executionId, Guid organizationId);
    Task ApproveActionAsync(Guid executionStepId, Guid organizationId, Guid approvedBy);
    Task RejectActionAsync(Guid executionStepId, Guid organizationId, Guid rejectedBy);
}

public interface IWorkflowExecutionLogger
{
    Task<WorkflowExecution> StartExecutionAsync(Guid workflowId, Guid organizationId, string triggeredBy, object eventData);
    Task UpdateExecutionStatusAsync(Guid executionId, WorkflowExecutionStatus status, string? errorMessage = null);
    
    Task<WorkflowExecutionStep> StartStepAsync(Guid executionId, Guid workflowStepId);
    Task UpdateStepStatusAsync(Guid stepId, WorkflowExecutionStatus status, string? output = null, string? errorMessage = null);
}

public class AIDecisionResult
{
    public string Decision { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public double Confidence { get; set; }
    public string RecommendedAction { get; set; } = string.Empty;
    public string RiskLevel { get; set; } = string.Empty;
    public string ExpectedBusinessImpact { get; set; } = string.Empty;
}
