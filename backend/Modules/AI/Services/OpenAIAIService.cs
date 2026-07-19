using System.ClientModel;
using backend.Entities;
using backend.Modules.AI.Interfaces;
using OpenAI.Chat;

namespace backend.Modules.AI.Services;

public class OpenAIAIService : IAIService
{
    private readonly IAIRepository _aiRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OpenAIAIService> _logger;

    public OpenAIAIService(IAIRepository aiRepository, IConfiguration configuration, ILogger<OpenAIAIService> logger)
    {
        _aiRepository = aiRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> ChatCompletionAsync(Guid organizationId, string systemPrompt, string userPrompt, string? model = null)
    {
        var apiKey = _configuration["AIProviders:OpenAI:ApiKey"];
        if (string.IsNullOrEmpty(apiKey)) throw new InvalidOperationException("OpenAI API Key is not configured.");

        var client = new ChatClient(model ?? "gpt-4o", apiKey);
        
        List<ChatMessage> messages = new()
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userPrompt)
        };

        var response = await client.CompleteChatAsync(messages);
        return response.Value.Content[0].Text;
    }

    public async Task<AIConversation> ProcessConversationTurnAsync(Guid organizationId, string userId, Guid conversationId, string userMessage)
    {
        var conversation = await _aiRepository.GetConversationAsync(organizationId, conversationId);
        if (conversation == null) throw new KeyNotFoundException("Conversation not found");

        var userMsgEntity = new AIMessage
        {
            ConversationId = conversation.Id,
            Role = "user",
            Content = userMessage
        };
        await _aiRepository.AddMessageAsync(userMsgEntity);

        var apiKey = _configuration["AIProviders:OpenAI:ApiKey"];
        var client = new ChatClient(conversation.Model, apiKey);

        var chatMessages = new List<ChatMessage> { new SystemChatMessage("You are a helpful business AI assistant.") };
        
        foreach(var msg in conversation.Messages.OrderBy(m => m.CreatedAt))
        {
            if (msg.Role == "user") chatMessages.Add(new UserChatMessage(msg.Content));
            else if (msg.Role == "assistant") chatMessages.Add(new AssistantChatMessage(msg.Content));
        }

        var response = await client.CompleteChatAsync(chatMessages);
        var aiResponseText = response.Value.Content[0].Text;

        var aiMsgEntity = new AIMessage
        {
            ConversationId = conversation.Id,
            Role = "assistant",
            Content = aiResponseText,
            TokenUsage = response.Value.Usage.TotalTokens
        };
        await _aiRepository.AddMessageAsync(aiMsgEntity);
        
        conversation.UpdatedAt = DateTime.UtcNow;
        await _aiRepository.UpdateConversationAsync(conversation);
        
        return await _aiRepository.GetConversationAsync(organizationId, conversationId) ?? conversation;
    }
}
