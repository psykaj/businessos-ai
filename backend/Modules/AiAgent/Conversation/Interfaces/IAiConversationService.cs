using backend.Modules.AiAgent.Conversation.DTOs;

namespace backend.Modules.AiAgent.Conversation.Interfaces;

public interface IAiConversationService
{
    Task<ConversationDto> CreateConversationAsync(Guid organizationId, string userId, CreateConversationRequestDto request, CancellationToken cancellationToken = default);
    Task<ConversationDto?> GetConversationByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<(List<ConversationDto> Items, int TotalCount)> GetConversationsPagedAsync(Guid organizationId, string? userId, int page, int pageSize, string? search, string? status, CancellationToken cancellationToken = default);
    Task<ConversationDto?> UpdateConversationAsync(Guid id, Guid organizationId, UpdateConversationRequestDto request, CancellationToken cancellationToken = default);
    Task<bool> ArchiveConversationAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<bool> DeleteConversationAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<MessageDto>> GetMessageHistoryAsync(Guid conversationId, Guid organizationId, CancellationToken cancellationToken = default);
}
