using System.Text.Json;
using backend.Modules.AiAgent.CommandEngine.Interfaces;
using backend.Modules.AiAgent.Context.Interfaces;
using backend.Modules.AiAgent.Entities;
using backend.Modules.AiAgent.Executions.DTOs;
using backend.Modules.AiAgent.Executions.Interfaces;
using backend.Modules.AiAgent.Permissions.Interfaces;
using backend.Modules.AiAgent.Repositories;
using backend.Modules.AiAgent.ToolRegistry.Interfaces;
using backend.Modules.AiAgent.ToolRegistry.Models;
using Microsoft.Extensions.Logging;

namespace backend.Modules.AiAgent.Executions.Services;

public class AiExecutionEngine : IAiExecutionEngine
{
    private readonly IAiContextEngine _contextEngine;
    private readonly IAiCommandEngine _commandEngine;
    private readonly IToolRegistry _toolRegistry;
    private readonly IAiPermissionSafetyService _safetyService;
    private readonly ICommandExecutionRepository _executionRepo;
    private readonly IAiConversationRepository _conversationRepo;
    private readonly IMessageRepository _messageRepo;
    private readonly ILogger<AiExecutionEngine> _logger;

    public AiExecutionEngine(
        IAiContextEngine contextEngine,
        IAiCommandEngine commandEngine,
        IToolRegistry toolRegistry,
        IAiPermissionSafetyService safetyService,
        ICommandExecutionRepository executionRepo,
        IAiConversationRepository conversationRepo,
        IMessageRepository messageRepo,
        ILogger<AiExecutionEngine> logger)
    {
        _contextEngine = contextEngine;
        _commandEngine = commandEngine;
        _toolRegistry = toolRegistry;
        _safetyService = safetyService;
        _executionRepo = executionRepo;
        _conversationRepo = conversationRepo;
        _messageRepo = messageRepo;
        _logger = logger;
    }

    public async Task<ExecutionResponseDto> ExecuteCommandAsync(
        Guid organizationId,
        string userId,
        ExecuteCommandRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var startedAt = DateTime.UtcNow;

        // 1. Build Organization & User Context
        var aiContext = await _contextEngine.BuildContextAsync(organizationId, userId, cancellationToken);

        // 2. Parse command intent
        var parsed = await _commandEngine.ParseCommandAsync(request.Command, aiContext, cancellationToken);
        var targetToolName = string.IsNullOrWhiteSpace(request.SpecifiedTool) ? parsed.ToolName : request.SpecifiedTool;
        var parameters = request.Parameters ?? parsed.Parameters;

        var execution = new CommandExecution
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            UserId = userId,
            Command = request.Command,
            ToolInvoked = targetToolName,
            Status = "Running",
            StartedAt = startedAt,
            ParametersJson = JsonSerializer.Serialize(parameters)
        };

        await _executionRepo.AddAsync(execution, cancellationToken);
        await _executionRepo.SaveChangesAsync(cancellationToken);

        // Record User Message if ConversationId present
        if (request.ConversationId.HasValue)
        {
            var userMsg = new Message
            {
                Id = Guid.NewGuid(),
                ConversationId = request.ConversationId.Value,
                Role = "User",
                Content = request.Command
            };
            await _messageRepo.AddAsync(userMsg, cancellationToken);
        }

        // 3. Resolve Tool
        var tool = _toolRegistry.GetTool(targetToolName);
        if (tool == null)
        {
            execution.Status = "Failed";
            execution.FinishedAt = DateTime.UtcNow;
            execution.ErrorMessage = $"Tool '{targetToolName}' not found in Tool Registry.";
            _executionRepo.Update(execution);
            await _executionRepo.SaveChangesAsync(cancellationToken);

            return new ExecutionResponseDto
            {
                ExecutionId = execution.Id,
                OrganizationId = organizationId,
                UserId = userId,
                Command = request.Command,
                ToolInvoked = targetToolName,
                Status = "Failed",
                StartedAt = startedAt,
                FinishedAt = execution.FinishedAt,
                ErrorMessage = execution.ErrorMessage,
                ResultSummary = execution.ErrorMessage,
                ConversationId = request.ConversationId
            };
        }

        // 4. Validate Permissions & Safety
        var validation = await _safetyService.ValidateExecutionAsync(tool, aiContext, parameters, request.IsConfirmed, cancellationToken);
        if (!validation.IsAllowed)
        {
            execution.Status = "Failed";
            execution.FinishedAt = DateTime.UtcNow;
            execution.ErrorMessage = validation.Reason;
            _executionRepo.Update(execution);
            await _executionRepo.SaveChangesAsync(cancellationToken);

            return new ExecutionResponseDto
            {
                ExecutionId = execution.Id,
                OrganizationId = organizationId,
                UserId = userId,
                Command = request.Command,
                ToolInvoked = targetToolName,
                Status = "Failed",
                StartedAt = startedAt,
                FinishedAt = execution.FinishedAt,
                ErrorMessage = validation.Reason,
                ResultSummary = validation.Reason ?? "Execution blocked by permission engine.",
                ConversationId = request.ConversationId
            };
        }

        if (validation.RequiresConfirmation && !request.IsConfirmed)
        {
            execution.Status = "RequiresConfirmation";
            execution.FinishedAt = DateTime.UtcNow;
            execution.ResultSummary = validation.Reason;
            _executionRepo.Update(execution);
            await _executionRepo.SaveChangesAsync(cancellationToken);

            return new ExecutionResponseDto
            {
                ExecutionId = execution.Id,
                OrganizationId = organizationId,
                UserId = userId,
                Command = request.Command,
                ToolInvoked = targetToolName,
                Status = "RequiresConfirmation",
                StartedAt = startedAt,
                FinishedAt = execution.FinishedAt,
                RequiresConfirmation = true,
                ResultSummary = validation.Reason ?? "Action requires explicit confirmation.",
                ConversationId = request.ConversationId
            };
        }

        // 5. Execute Tool
        var toolExecContext = new ToolExecutionContext
        {
            OrganizationId = organizationId,
            UserId = userId,
            UserRoles = aiContext.UserRoles,
            UserPermissions = aiContext.UserPermissions,
            IsConfirmed = request.IsConfirmed
        };

        var toolResult = await tool.ExecuteAsync(toolExecContext, parameters, cancellationToken);

        execution.FinishedAt = DateTime.UtcNow;
        execution.Status = toolResult.Success ? "Success" : "Failed";
        execution.ResultSummary = toolResult.Message;

        _executionRepo.Update(execution);
        await _executionRepo.SaveChangesAsync(cancellationToken);

        // Record Assistant Response Message
        if (request.ConversationId.HasValue)
        {
            var assistantMsg = new Message
            {
                Id = Guid.NewGuid(),
                ConversationId = request.ConversationId.Value,
                Role = "Assistant",
                Content = toolResult.Message,
                ToolInvoked = targetToolName,
                ExecutionId = execution.Id.ToString()
            };
            await _messageRepo.AddAsync(assistantMsg, cancellationToken);
            await _messageRepo.SaveChangesAsync(cancellationToken);
        }

        return new ExecutionResponseDto
        {
            ExecutionId = execution.Id,
            OrganizationId = organizationId,
            UserId = userId,
            Command = request.Command,
            ToolInvoked = targetToolName,
            Status = execution.Status,
            StartedAt = startedAt,
            FinishedAt = execution.FinishedAt,
            ResultSummary = toolResult.Message,
            Data = toolResult.Data,
            RequiresConfirmation = toolResult.RequiresConfirmation,
            ConversationId = request.ConversationId
        };
    }
}
