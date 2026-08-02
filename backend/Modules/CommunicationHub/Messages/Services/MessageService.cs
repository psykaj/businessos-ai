using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.Modules.CommunicationHub.Channels.DTOs;
using backend.Modules.CommunicationHub.Channels.Interfaces;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Messages.DTOs;
using backend.Modules.CommunicationHub.Messages.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Messages.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repository;
    private readonly ApplicationDbContext _dbContext;
    private readonly IChannelService _channelService;
    private readonly IMapper _mapper;
    private readonly ILogger<MessageService> _logger;

    public MessageService(
        IMessageRepository repository,
        ApplicationDbContext dbContext,
        IChannelService channelService,
        IMapper mapper,
        ILogger<MessageService> logger)
    {
        _repository = repository;
        _dbContext = dbContext;
        _channelService = channelService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<MessageDto>> GetMessagesAsync(Guid conversationId, Guid organizationId)
    {
        var messages = await _repository.GetByConversationIdAsync(conversationId, organizationId);
        return _mapper.Map<IEnumerable<MessageDto>>(messages);
    }

    public async Task<MessageDto?> SendMessageAsync(Guid organizationId, SendMessageRequest request, Guid senderUserId, string senderName)
    {
        var conversation = await _dbContext.CommConversations
            .FirstOrDefaultAsync(c => c.Id == request.ConversationId && c.OrganizationId == organizationId && !c.IsDeleted);

        if (conversation == null) return null;

        var message = new Message
        {
            OrganizationId = organizationId,
            ConversationId = conversation.Id,
            Direction = MessageDirection.Outbound,
            Content = request.Content,
            AttachmentsJson = request.AttachmentsJson,
            QuickReplyMetadataJson = request.QuickReplyMetadataJson,
            SenderId = senderUserId,
            SenderName = senderName,
            Status = DeliveryStatus.Sending,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.CreateAsync(message);

        // Dispatch via provider-independent Channel Engine
        var outboundReq = new OutboundMessageRequest
        {
            ConversationId = conversation.Id,
            RecipientIdentifier = conversation.CustomerEmail ?? conversation.CustomerPhone ?? conversation.CustomerName,
            Content = message.Content,
            AttachmentsJson = message.AttachmentsJson,
            ChannelType = conversation.ChannelType
        };

        try
        {
            var result = await _channelService.SendOutboundMessageAsync(outboundReq);
            message.Status = result.Success ? result.Status : DeliveryStatus.Failed;
            message.ExternalMessageId = result.ExternalMessageId;
            if (result.Success)
            {
                message.DeliveredAt = DateTime.UtcNow;
            }
            await _repository.UpdateRangeAsync(new[] { message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error dispatching outbound message via channel service.");
            message.Status = DeliveryStatus.Failed;
            await _repository.UpdateRangeAsync(new[] { message });
        }

        // Update conversation summary
        conversation.LastMessageAt = DateTime.UtcNow;
        conversation.LastMessagePreview = message.Content.Length > 200 ? message.Content.Substring(0, 197) + "..." : message.Content;
        _dbContext.CommConversations.Update(conversation);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Sent message {MsgId} for conversation {ConvId} via {Channel}", message.Id, conversation.Id, conversation.ChannelType);
        return _mapper.Map<MessageDto>(message);
    }

    public async Task<MessageDto?> AddInternalNoteAsync(Guid organizationId, AddInternalNoteRequest request, Guid authorUserId, string authorName)
    {
        var conversation = await _dbContext.CommConversations
            .FirstOrDefaultAsync(c => c.Id == request.ConversationId && c.OrganizationId == organizationId && !c.IsDeleted);

        if (conversation == null) return null;

        var note = new Message
        {
            OrganizationId = organizationId,
            ConversationId = conversation.Id,
            Direction = MessageDirection.InternalNote,
            Content = request.Content,
            AttachmentsJson = request.AttachmentsJson,
            SenderId = authorUserId,
            SenderName = $"[Internal Note] {authorName}",
            Status = DeliveryStatus.Read,
            ReadAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(note);
        _logger.LogInformation("Added internal note {Id} to conversation {ConvId}", created.Id, conversation.Id);
        return _mapper.Map<MessageDto>(created);
    }

    public async Task<bool> MarkConversationMessagesAsReadAsync(Guid conversationId, Guid organizationId)
    {
        var unread = await _repository.GetUnreadInboundMessagesAsync(conversationId, organizationId);
        var now = DateTime.UtcNow;
        foreach (var msg in unread)
        {
            msg.ReadAt = now;
            msg.Status = DeliveryStatus.Read;
        }
        await _repository.UpdateRangeAsync(unread);

        // Clear conversation unread counter
        var conversation = await _dbContext.CommConversations
            .FirstOrDefaultAsync(c => c.Id == conversationId && c.OrganizationId == organizationId);
        if (conversation != null && conversation.UnreadMessagesCount > 0)
        {
            conversation.UnreadMessagesCount = 0;
            _dbContext.CommConversations.Update(conversation);
            await _dbContext.SaveChangesAsync();
        }

        return true;
    }

    public async Task<bool> DeleteMessageAsync(Guid id, Guid organizationId)
    {
        var msg = await _repository.GetByIdAsync(id, organizationId);
        if (msg == null) return false;
        await _repository.DeleteAsync(msg);
        return true;
    }
}
