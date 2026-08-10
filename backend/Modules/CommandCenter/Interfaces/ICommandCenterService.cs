using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.CommandCenter.DTOs;
using backend.Modules.AiAgent.Executions.DTOs;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.ActionCenter.DTOs;

namespace backend.Modules.CommandCenter.Interfaces;

public interface ICommandCenterService
{
    Task<CommandCenterSummaryDto> GetCommandCenterSummaryAsync(Guid organizationId, string userId, CancellationToken cancellationToken = default);
    Task<List<CommandMetricDto>> GetMetricsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<ProactiveAlertDto>> GetAlertsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<CommandOpportunityDto>> GetOpportunitiesAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<AiActionDto>> GetActionsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<CommandAutomationSummaryDto> GetAutomationsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<CommandActivityDto>> GetActivityAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<CommandTrendDto>> GetTrendsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<CommandCenterAskResponseDto> AskBusinessOsAiAsync(Guid organizationId, string userId, CommandCenterAskRequestDto request, CancellationToken cancellationToken = default);
}
