using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Inbox.DTOs;
using backend.Modules.CommunicationHub.Inbox.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CommunicationHub.Inbox.Repositories;

public class InboxRepository : IInboxRepository
{
    private readonly ApplicationDbContext _context;

    public InboxRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InboxSummaryDto> GetSummaryAsync(Guid organizationId)
    {
        var activeStatus = new[] { "Resolved", "Closed" };
        var query = _context.CommConversations
            .Where(c => c.OrganizationId == organizationId && !c.IsDeleted && !activeStatus.Contains(c.Status));

        var totalActive = await query.CountAsync();
        var totalUnread = await query.Where(c => c.UnreadMessagesCount > 0).CountAsync();
        var totalSlaBreached = await query.Where(c => c.IsSlaBreached).CountAsync();
        var totalUrgent = await query.Where(c => c.Priority == ConversationPriority.Urgent).CountAsync();

        var channelCounts = new List<ChannelBadgeCountDto>();
        foreach (CommunicationChannelType type in Enum.GetValues(typeof(CommunicationChannelType)))
        {
            var channelActive = await query.Where(c => c.ChannelType == type).CountAsync();
            var channelUnread = await query.Where(c => c.ChannelType == type && c.UnreadMessagesCount > 0).CountAsync();
            if (channelActive > 0 || channelUnread > 0)
            {
                channelCounts.Add(new ChannelBadgeCountDto
                {
                    ChannelType = type,
                    ActiveCount = channelActive,
                    UnreadCount = channelUnread
                });
            }
        }

        return new InboxSummaryDto
        {
            OrganizationId = organizationId,
            TotalActiveConversations = totalActive,
            TotalUnreadConversations = totalUnread,
            TotalSlaBreached = totalSlaBreached,
            TotalUrgentConversations = totalUrgent,
            ChannelCounts = channelCounts,
            LastRefreshedAt = DateTime.UtcNow
        };
    }
}
