using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.ActionCenter.Entities;
using backend.Modules.ActionCenter.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace backend.Modules.ActionCenter.Commands;

public class ActionCommandHandlers :
    IRequestHandler<ExecuteActionCommand, bool>,
    IRequestHandler<ApproveActionCommand, bool>,
    IRequestHandler<RejectActionCommand, bool>
{
    private readonly IAiActionRepository _repository;
    private readonly ILogger<ActionCommandHandlers> _logger;

    public ActionCommandHandlers(IAiActionRepository repository, ILogger<ActionCommandHandlers> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> Handle(ExecuteActionCommand request, CancellationToken cancellationToken)
    {
        var action = await _repository.GetByIdAsync(request.ActionId, request.OrganizationId);
        if (action == null)
            throw new Exception("Action not found");

        if (action.Status == AiActionStatus.Executed || action.Status == AiActionStatus.Failed)
            throw new InvalidOperationException("Action has already been processed.");

        if (action.RiskLevel == AiActionRiskLevel.High && action.Status != AiActionStatus.Approved)
            throw new InvalidOperationException("High-risk actions must be approved before execution.");

        try
        {
            // Simulate actual execution logic here
            // In a real system, you might publish an event via MassTransit/EventBus to perform the action.
            
            action.Status = AiActionStatus.Executed;
            action.ExecutedDate = DateTime.UtcNow;
            action.ExecutionResult = "Executed successfully.";
            
            await _repository.UpdateAsync(action);
            _logger.LogInformation("Action {ActionId} executed successfully by user {UserId}", action.Id, request.UserId);
            
            return true;
        }
        catch (Exception ex)
        {
            action.Status = AiActionStatus.Failed;
            action.ExecutionResult = $"Failed: {ex.Message}";
            await _repository.UpdateAsync(action);
            _logger.LogError(ex, "Failed to execute action {ActionId}", action.Id);
            throw;
        }
    }

    public async Task<bool> Handle(ApproveActionCommand request, CancellationToken cancellationToken)
    {
        var action = await _repository.GetByIdAsync(request.ActionId, request.OrganizationId);
        if (action == null)
            throw new Exception("Action not found");

        if (action.Status != AiActionStatus.Pending)
            throw new InvalidOperationException("Only pending actions can be approved.");

        action.Status = AiActionStatus.Approved;
        action.ApprovedBy = request.UserId;
        
        await _repository.UpdateAsync(action);
        _logger.LogInformation("Action {ActionId} approved by user {UserId}", action.Id, request.UserId);
        
        return true;
    }

    public async Task<bool> Handle(RejectActionCommand request, CancellationToken cancellationToken)
    {
        var action = await _repository.GetByIdAsync(request.ActionId, request.OrganizationId);
        if (action == null)
            throw new Exception("Action not found");

        if (action.Status != AiActionStatus.Pending && action.Status != AiActionStatus.Approved)
            throw new InvalidOperationException("Only pending or approved actions can be rejected.");

        action.Status = AiActionStatus.Rejected;
        
        await _repository.UpdateAsync(action);
        _logger.LogInformation("Action {ActionId} rejected by user {UserId}", action.Id, request.UserId);
        
        return true;
    }
}
