using backend.Modules.AiAgent.Entities;

namespace backend.Modules.AiAgent.Repositories;

public interface IMessageRepository
{
    Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Message>> GetByConversationIdAsync(Guid conversationId, CancellationToken cancellationToken = default);
    Task AddAsync(Message message, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
