using System;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Channels.Interfaces;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Inbox.DTOs;
using backend.Modules.CommunicationHub.Inbox.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Inbox.Services;

public class InboxService : IInboxService
{
    private readonly IInboxRepository _repository;
    private readonly IChannelAdapterFactory _adapterFactory;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<InboxService> _logger;

    public InboxService(
        IInboxRepository repository,
        IChannelAdapterFactory adapterFactory,
        ApplicationDbContext dbContext,
        ILogger<InboxService> logger)
    {
        _repository = repository;
        _adapterFactory = adapterFactory;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<InboxSummaryDto> GetInboxSummaryAsync(Guid organizationId)
    {
        return await _repository.GetSummaryAsync(organizationId);
    }

    public async Task<WebhookIngestResponse> ProcessInboundWebhookAsync(Guid organizationId, string channelTypeStr, string payload, string? signature)
    {
        if (!Enum.TryParse<CommunicationChannelType>(channelTypeStr, true, out var channelType))
        {
            return new WebhookIngestResponse { ProcessedSuccessfully = false, Error = $"Unknown communication channel type: {channelTypeStr}" };
        }

        try
        {
            var adapter = _adapterFactory.GetAdapter(channelType);
            var parsed = await adapter.ParseInboundWebhookAsync(payload, signature);
            if (parsed == null)
            {
                return new WebhookIngestResponse { ProcessedSuccessfully = false, Error = "Failed to parse webhook payload or invalid signature." };
            }

            // Look for existing active conversation from this sender
            var activeStatus = new[] { "Resolved", "Closed" };
            var conversation = await _dbContext.CommConversations
                .FirstOrDefaultAsync(c => c.OrganizationId == organizationId && 
                                          c.ChannelType == channelType &&
                                          !c.IsDeleted &&
                                          !activeStatus.Contains(c.Status) &&
                                          (c.CustomerEmail == parsed.SenderIdentifier || c.CustomerPhone == parsed.SenderIdentifier || c.CustomerName.Contains(parsed.SenderIdentifier)));

            var now = DateTime.UtcNow;
            if (conversation == null)
            {
                conversation = new Conversation
                {
                    OrganizationId = organizationId,
                    ChannelType = channelType,
                    Subject = $"Inbound via {channelType} ({parsed.SenderName})",
                    CustomerName = parsed.SenderName,
                    CustomerPhone = channelType == CommunicationChannelType.WhatsApp || channelType == CommunicationChannelType.SMS ? parsed.SenderIdentifier : null,
                    CustomerEmail = channelType == CommunicationChannelType.Email ? parsed.SenderIdentifier : null,
                    Status = "New",
                    Priority = ConversationPriority.Medium,
                    UnreadMessagesCount = 1,
                    LastMessageAt = now,
                    LastMessagePreview = parsed.Content.Length > 200 ? parsed.Content.Substring(0, 197) + "..." : parsed.Content,
                    SlaDueDate = now.AddHours(2),
                    CreatedAt = now
                };
                await _dbContext.CommConversations.AddAsync(conversation);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Created new conversation {ConvId} from inbound webhook for {Channel}", conversation.Id, channelType);
            }
            else
            {
                conversation.UnreadMessagesCount++;
                conversation.LastMessageAt = now;
                conversation.LastMessagePreview = parsed.Content.Length > 200 ? parsed.Content.Substring(0, 197) + "..." : parsed.Content;
                _dbContext.CommConversations.Update(conversation);
            }

            var message = new Message
            {
                OrganizationId = organizationId,
                ConversationId = conversation.Id,
                Direction = MessageDirection.Inbound,
                Content = parsed.Content,
                AttachmentsJson = parsed.AttachmentsJson,
                SenderName = parsed.SenderName,
                Status = DeliveryStatus.Delivered,
                DeliveredAt = now,
                ExternalMessageId = parsed.ExternalMessageId,
                CreatedAt = now
            };

            await _dbContext.CommMessages.AddAsync(message);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Processed inbound message {MsgId} in conversation {ConvId}", message.Id, conversation.Id);
            return new WebhookIngestResponse { ProcessedSuccessfully = true, ConversationId = conversation.Id, MessageId = message.Id };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing inbound webhook for organization {OrgId}", organizationId);
            return new WebhookIngestResponse { ProcessedSuccessfully = false, Error = ex.Message };
        }
    }
}
