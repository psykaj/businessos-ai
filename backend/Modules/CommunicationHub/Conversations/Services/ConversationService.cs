using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using backend.Modules.CommunicationHub.Conversations.DTOs;
using backend.Modules.CommunicationHub.Conversations.Interfaces;
using backend.Modules.CommunicationHub.Entities;
using backend.Persistence;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Conversations.Services;

public class ConversationService : IConversationService
{
    private readonly IConversationRepository _repository;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<ConversationService> _logger;

    public ConversationService(
        IConversationRepository repository,
        ApplicationDbContext dbContext,
        IMapper mapper,
        ILogger<ConversationService> logger)
    {
        _repository = repository;
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<ConversationDto> Items, int TotalCount)> GetConversationsAsync(Guid organizationId, ConversationFilterDto filter)
    {
        var (items, total) = await _repository.GetFilteredAsync(organizationId, filter);
        return (_mapper.Map<IEnumerable<ConversationDto>>(items), total);
    }

    public async Task<ConversationDto?> GetConversationByIdAsync(Guid id, Guid organizationId)
    {
        var item = await _repository.GetByIdAsync(id, organizationId);
        if (item == null) return null;
        return _mapper.Map<ConversationDto>(item);
    }

    public async Task<ConversationDto> CreateConversationAsync(Guid organizationId, CreateConversationRequest request, Guid currentUserId, string currentUserName)
    {
        var now = DateTime.UtcNow;
        var conversation = new Conversation
        {
            OrganizationId = organizationId,
            ChannelType = request.ChannelType,
            ChannelId = request.ChannelId,
            Subject = request.Subject,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            CustomerPhone = request.CustomerPhone,
            Priority = request.Priority,
            Status = "Open",
            LastMessageAt = now,
            LastMessagePreview = request.InitialMessageContent.Length > 200 
                ? request.InitialMessageContent.Substring(0, 197) + "..." : request.InitialMessageContent,
            SlaDueDate = now.AddHours(2) // Default SLA due in 2 hours
        };

        var created = await _repository.CreateAsync(conversation);

        // If initial message content provided, log it as first outbound or notes
        if (!string.IsNullOrWhiteSpace(request.InitialMessageContent))
        {
            var initialMsg = new Message
            {
                OrganizationId = organizationId,
                ConversationId = created.Id,
                Direction = MessageDirection.Outbound,
                Content = request.InitialMessageContent,
                SenderId = currentUserId,
                SenderName = currentUserName,
                Status = DeliveryStatus.Sent,
                CreatedAt = now
            };
            await _dbContext.CommMessages.AddAsync(initialMsg);
            await _dbContext.SaveChangesAsync();
        }

        _logger.LogInformation("Created new conversation {ConvId} for organization {OrgId}", created.Id, organizationId);
        return _mapper.Map<ConversationDto>(created);
    }

    public async Task<ConversationDto?> UpdateStatusAsync(Guid id, Guid organizationId, string newStatus)
    {
        var item = await _repository.GetByIdAsync(id, organizationId);
        if (item == null) return null;

        item.Status = newStatus;
        if (newStatus.Equals("Resolved", StringComparison.OrdinalIgnoreCase) || newStatus.Equals("Closed", StringComparison.OrdinalIgnoreCase))
        {
            item.ResolvedAt = DateTime.UtcNow;
        }

        await _repository.UpdateAsync(item);
        return _mapper.Map<ConversationDto>(item);
    }

    public async Task<ConversationDto?> AssignConversationAsync(
        Guid id, 
        Guid organizationId, 
        Guid assignedToUserId, 
        string assignedToUserName, 
        Guid assignedByUserId, 
        string assignedByUserName, 
        string? reason)
    {
        var conversation = await _repository.GetByIdAsync(id, organizationId);
        if (conversation == null) return null;

        conversation.AssignedToUserId = assignedToUserId;
        conversation.AssignedToUserName = assignedToUserName;

        // Log assignment history
        var assignment = new Assignment
        {
            OrganizationId = organizationId,
            ConversationId = conversation.Id,
            AssignedToUserId = assignedToUserId,
            AssignedToUserName = assignedToUserName,
            AssignedByUserId = assignedByUserId,
            AssignedByUserName = assignedByUserName,
            AssignmentReason = reason,
            Status = AssignmentStatus.Active,
            AssignedAt = DateTime.UtcNow
        };
        await _dbContext.CommAssignments.AddAsync(assignment);
        await _repository.UpdateAsync(conversation);

        _logger.LogInformation("Assigned conversation {Id} to user {UserId}", conversation.Id, assignedToUserId);
        return _mapper.Map<ConversationDto>(conversation);
    }

    public async Task<ConversationDto?> UpdateTagsAsync(Guid id, Guid organizationId, string tags)
    {
        var item = await _repository.GetByIdAsync(id, organizationId);
        if (item == null) return null;

        item.Tags = tags;
        await _repository.UpdateAsync(item);
        return _mapper.Map<ConversationDto>(item);
    }

    public async Task<bool> DeleteConversationAsync(Guid id, Guid organizationId)
    {
        var item = await _repository.GetByIdAsync(id, organizationId);
        if (item == null) return false;

        await _repository.DeleteAsync(item);
        return true;
    }

    public async Task CheckAndMarkSlaBreachesAsync()
    {
        var breaches = await _repository.GetOpenConversationsForSlaCheckAsync();
        foreach (var conv in breaches)
        {
            conv.IsSlaBreached = true;
            _dbContext.CommConversations.Update(conv);

            // Trigger internal notification for breached SLA
            if (conv.AssignedToUserId.HasValue && conv.AssignedToUserId.Value != Guid.Empty)
            {
                var notif = new CommunicationNotification
                {
                    OrganizationId = conv.OrganizationId,
                    TargetUserId = conv.AssignedToUserId.Value,
                    ConversationId = conv.Id,
                    ChannelType = conv.ChannelType,
                    Title = "SLA Breached!",
                    Content = $"Conversation '{conv.Subject}' with {conv.CustomerName} has exceeded its SLA response target.",
                    NotificationType = CommunicationNotificationType.SlaBreached,
                    IsRead = false
                };
                await _dbContext.CommNotifications.AddAsync(notif);
            }
        }
        await _dbContext.SaveChangesAsync();
        if (breaches.Any())
        {
            _logger.LogWarning("Marked {Count} conversations as SLA Breached.", breaches.Count());
        }
    }
}
