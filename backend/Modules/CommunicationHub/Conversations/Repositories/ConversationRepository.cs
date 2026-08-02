using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Conversations.DTOs;
using backend.Modules.CommunicationHub.Conversations.Interfaces;
using backend.Modules.CommunicationHub.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CommunicationHub.Conversations.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly ApplicationDbContext _context;

    public ConversationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Conversation> Items, int TotalCount)> GetFilteredAsync(Guid organizationId, ConversationFilterDto filter)
    {
        var query = _context.CommConversations
            .Where(c => c.OrganizationId == organizationId && !c.IsDeleted);

        if (filter.ChannelType.HasValue)
        {
            query = query.Where(c => c.ChannelType == filter.ChannelType.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            query = query.Where(c => c.Status.ToLower() == filter.Status.ToLower());
        }

        if (filter.AssigneeId.HasValue)
        {
            query = query.Where(c => c.AssignedToUserId == filter.AssigneeId.Value);
        }

        if (filter.Priority.HasValue)
        {
            query = query.Where(c => c.Priority == filter.Priority.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchKeyword))
        {
            var kw = filter.SearchKeyword.ToLower();
            query = query.Where(c => c.Subject.ToLower().Contains(kw) || 
                                     c.CustomerName.ToLower().Contains(kw) || 
                                     c.LastMessagePreview.ToLower().Contains(kw));
        }

        int totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(c => c.LastMessageAt)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Conversation?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.CommConversations
            .FirstOrDefaultAsync(c => c.Id == id && c.OrganizationId == organizationId && !c.IsDeleted);
    }

    public async Task<IEnumerable<Conversation>> GetOpenConversationsForSlaCheckAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.CommConversations
            .Where(c => !c.IsDeleted && !c.IsSlaBreached && c.Status != "Resolved" && c.Status != "Closed" && c.SlaDueDate != null && c.SlaDueDate <= now)
            .ToListAsync();
    }

    public async Task<Conversation> CreateAsync(Conversation conversation)
    {
        await _context.CommConversations.AddAsync(conversation);
        await _context.SaveChangesAsync();
        return conversation;
    }

    public async Task<Conversation> UpdateAsync(Conversation conversation)
    {
        _context.CommConversations.Update(conversation);
        await _context.SaveChangesAsync();
        return conversation;
    }

    public async Task DeleteAsync(Conversation conversation)
    {
        _context.CommConversations.Remove(conversation);
        await _context.SaveChangesAsync();
    }
}
