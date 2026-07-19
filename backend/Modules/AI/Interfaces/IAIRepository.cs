using backend.Entities;

namespace backend.Modules.AI.Interfaces;

public interface IAIRepository
{
    Task<AIConversation?> GetConversationAsync(Guid organizationId, Guid conversationId);
    Task<IEnumerable<AIConversation>> GetConversationsAsync(Guid organizationId, string userId);
    Task<AIConversation> CreateConversationAsync(AIConversation conversation);
    Task UpdateConversationAsync(AIConversation conversation);
    
    Task<AIMessage> AddMessageAsync(AIMessage message);
    Task<IEnumerable<AIMessage>> GetMessagesAsync(Guid conversationId);
}
