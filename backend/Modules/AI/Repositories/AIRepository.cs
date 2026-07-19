using backend.Entities;
using backend.Modules.AI.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AI.Repositories;

public class AIRepository : IAIRepository
{
    private readonly ApplicationDbContext _context;

    public AIRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AIConversation?> GetConversationAsync(Guid organizationId, Guid conversationId)
    {
        return await _context.AIConversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.OrganizationId == organizationId && c.Id == conversationId);
    }

    public async Task<IEnumerable<AIConversation>> GetConversationsAsync(Guid organizationId, string userId)
    {
        return await _context.AIConversations
            .Where(c => c.OrganizationId == organizationId && c.UserId == userId && !c.IsDeleted)
            .OrderByDescending(c => c.UpdatedAt)
            .ToListAsync();
    }

    public async Task<AIConversation> CreateConversationAsync(AIConversation conversation)
    {
        _context.AIConversations.Add(conversation);
        await _context.SaveChangesAsync();
        return conversation;
    }

    public async Task UpdateConversationAsync(AIConversation conversation)
    {
        _context.AIConversations.Update(conversation);
        await _context.SaveChangesAsync();
    }

    public async Task<AIMessage> AddMessageAsync(AIMessage message)
    {
        _context.AIMessages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<IEnumerable<AIMessage>> GetMessagesAsync(Guid conversationId)
    {
        return await _context.AIMessages
            .Where(m => m.ConversationId == conversationId && !m.IsDeleted)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }
}
