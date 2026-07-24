using ConversationEntity = backend.Modules.AiAgent.Entities.Conversation;

namespace backend.Modules.AiAgent.Repositories;

public interface IAiConversationRepository
{
    Task<ConversationEntity?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<ConversationEntity?> GetByIdWithMessagesAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<ConversationEntity>> GetAllByOrganizationIdAsync(Guid organizationId, string? userId = null, CancellationToken cancellationToken = default);
    Task<(List<ConversationEntity> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId,
        string? userId = null,
        int page = 1,
        int pageSize = 20,
        string? search = null,
        string? status = null,
        CancellationToken cancellationToken = default);
    Task AddAsync(ConversationEntity conversation, CancellationToken cancellationToken = default);
    void Update(ConversationEntity conversation);
    void Delete(ConversationEntity conversation);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
