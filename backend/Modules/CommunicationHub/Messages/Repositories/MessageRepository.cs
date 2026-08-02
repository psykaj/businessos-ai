using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Messages.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CommunicationHub.Messages.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationDbContext _context;

    public MessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Message>> GetByConversationIdAsync(Guid conversationId, Guid organizationId)
    {
        return await _context.CommMessages
            .Where(m => m.ConversationId == conversationId && m.OrganizationId == organizationId && !m.IsDeleted)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<Message?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.CommMessages
            .FirstOrDefaultAsync(m => m.Id == id && m.OrganizationId == organizationId && !m.IsDeleted);
    }

    public async Task<Message> CreateAsync(Message message)
    {
        await _context.CommMessages.AddAsync(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<IEnumerable<Message>> GetUnreadInboundMessagesAsync(Guid conversationId, Guid organizationId)
    {
        return await _context.CommMessages
            .Where(m => m.ConversationId == conversationId && 
                        m.OrganizationId == organizationId && 
                        m.Direction == MessageDirection.Inbound && 
                        m.ReadAt == null && 
                        !m.IsDeleted)
            .ToListAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<Message> messages)
    {
        _context.CommMessages.UpdateRange(messages);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Message message)
    {
        _context.CommMessages.Remove(message);
        await _context.SaveChangesAsync();
    }
}
