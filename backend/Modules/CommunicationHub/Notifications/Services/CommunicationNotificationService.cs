using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Notifications.DTOs;
using backend.Modules.CommunicationHub.Notifications.Interfaces;
using backend.Modules.CommunicationHub.RealTime;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Notifications.Services;

public class CommunicationNotificationService : ICommunicationNotificationService
{
    private readonly ICommunicationNotificationRepository _repository;
    private readonly IHubContext<CommunicationHubSignalR> _hubContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CommunicationNotificationService> _logger;

    public CommunicationNotificationService(
        ICommunicationNotificationRepository repository,
        IHubContext<CommunicationHubSignalR> hubContext,
        IMapper mapper,
        ILogger<CommunicationNotificationService> logger)
    {
        _repository = repository;
        _hubContext = hubContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<CommunicationNotificationDto>> GetUserNotificationsAsync(Guid organizationId, Guid targetUserId, bool unreadOnly)
    {
        var items = await _repository.GetByUserIdAsync(organizationId, targetUserId, unreadOnly);
        return _mapper.Map<IEnumerable<CommunicationNotificationDto>>(items);
    }

    public async Task<CommunicationNotificationDto?> MarkAsReadAsync(Guid id, Guid organizationId)
    {
        var item = await _repository.GetByIdAsync(id, organizationId);
        if (item == null) return null;

        item.IsRead = true;
        item.ReadAt = DateTime.UtcNow;
        await _repository.UpdateAsync(item);

        return _mapper.Map<CommunicationNotificationDto>(item);
    }

    public async Task MarkAllAsReadAsync(Guid organizationId, Guid targetUserId)
    {
        await _repository.MarkAllAsReadAsync(organizationId, targetUserId);
    }

    public async Task<CommunicationNotificationDto> SendNotificationAsync(
        Guid organizationId, 
        Guid targetUserId, 
        string title, 
        string content, 
        Guid? conversationId, 
        CommunicationChannelType? channelType, 
        CommunicationNotificationType type)
    {
        var notification = new CommunicationNotification
        {
            OrganizationId = organizationId,
            TargetUserId = targetUserId,
            Title = title,
            Content = content,
            ConversationId = conversationId,
            ChannelType = channelType,
            NotificationType = type,
            IsRead = false
        };

        await _repository.CreateAsync(notification);
        var dto = _mapper.Map<CommunicationNotificationDto>(notification);

        // Emit real-time notification via dedicated communication SignalR Hub
        try
        {
            await _hubContext.Clients.Group($"CommUser_{targetUserId}").SendAsync("ReceiveCommunicationNotification", dto);
            _logger.LogInformation("Real-time communication notification sent to user {UserId}", targetUserId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to broadcast notification over SignalR to group CommUser_{UserId}", targetUserId);
        }

        return dto;
    }
}
