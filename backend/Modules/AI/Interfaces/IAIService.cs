using backend.Entities;

namespace backend.Modules.AI.Interfaces;

public interface IAIService
{
    Task<string> ChatCompletionAsync(Guid organizationId, string systemPrompt, string userPrompt, string? model = null);
    Task<AIConversation> ProcessConversationTurnAsync(Guid organizationId, string userId, Guid conversationId, string userMessage);
}
