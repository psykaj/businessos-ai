using System;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Inbox.DTOs;

namespace backend.Modules.CommunicationHub.Inbox.Interfaces;

public interface IInboxService
{
    Task<InboxSummaryDto> GetInboxSummaryAsync(Guid organizationId);
    Task<WebhookIngestResponse> ProcessInboundWebhookAsync(Guid organizationId, string channelTypeStr, string payload, string? signature);
}
