using backend.Modules.AiAgent.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AiAgent.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationDbContext _context;

    public MessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Messages
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted, cancellationToken);
    }

    public async Task<List<Message>> GetByConversationIdAsync(Guid conversationId, CancellationToken cancellationToken = default)
    {
        return await _context.Messages
            .AsNoTracking()
            .Where(m => m.ConversationId == conversationId && !m.IsDeleted)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Message message, CancellationToken cancellationToken = default)
    {
        await _context.Messages.AddAsync(message, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
