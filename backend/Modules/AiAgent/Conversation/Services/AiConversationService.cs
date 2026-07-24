using ConversationEntity = backend.Modules.AiAgent.Entities.Conversation;
using backend.Modules.AiAgent.Conversation.DTOs;
using backend.Modules.AiAgent.Conversation.Interfaces;
using backend.Modules.AiAgent.Repositories;

namespace backend.Modules.AiAgent.Conversation.Services;

public class AiConversationService : IAiConversationService
{
    private readonly IAiConversationRepository _conversationRepo;
    private readonly IMessageRepository _messageRepo;

    public AiConversationService(
        IAiConversationRepository conversationRepo,
        IMessageRepository messageRepo)
    {
        _conversationRepo = conversationRepo;
        _messageRepo = messageRepo;
    }

    public async Task<ConversationDto> CreateConversationAsync(Guid organizationId, string userId, CreateConversationRequestDto request, CancellationToken cancellationToken = default)
    {
        var conversation = new ConversationEntity
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            UserId = userId,
            Title = string.IsNullOrWhiteSpace(request.Title) ? "New Business Conversation" : request.Title,
            Status = "Active"
        };

        await _conversationRepo.AddAsync(conversation, cancellationToken);
        await _conversationRepo.SaveChangesAsync(cancellationToken);

        return new ConversationDto
        {
            Id = conversation.Id,
            OrganizationId = conversation.OrganizationId,
            UserId = conversation.UserId,
            Title = conversation.Title,
            Status = conversation.Status,
            CreatedAt = conversation.CreatedAt,
            UpdatedAt = conversation.UpdatedAt,
            MessageCount = 0
        };
    }

    public async Task<ConversationDto?> GetConversationByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var conversation = await _conversationRepo.GetByIdWithMessagesAsync(id, organizationId, cancellationToken);
        if (conversation == null) return null;

        return new ConversationDto
        {
            Id = conversation.Id,
            OrganizationId = conversation.OrganizationId,
            UserId = conversation.UserId,
            Title = conversation.Title,
            Status = conversation.Status,
            CreatedAt = conversation.CreatedAt,
            UpdatedAt = conversation.UpdatedAt,
            MessageCount = conversation.Messages.Count,
            Messages = conversation.Messages.Select(m => new MessageDto
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                Role = m.Role,
                Content = m.Content,
                ToolInvoked = m.ToolInvoked,
                ExecutionId = m.ExecutionId,
                CreatedAt = m.CreatedAt
            }).ToList()
        };
    }

    public async Task<(List<ConversationDto> Items, int TotalCount)> GetConversationsPagedAsync(Guid organizationId, string? userId, int page, int pageSize, string? search, string? status, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _conversationRepo.GetPagedAsync(organizationId, userId, page, pageSize, search, status, cancellationToken);

        var dtos = items.Select(c => new ConversationDto
        {
            Id = c.Id,
            OrganizationId = c.OrganizationId,
            UserId = c.UserId,
            Title = c.Title,
            Status = c.Status,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            MessageCount = c.Messages.Count
        }).ToList();

        return (dtos, totalCount);
    }

    public async Task<ConversationDto?> UpdateConversationAsync(Guid id, Guid organizationId, UpdateConversationRequestDto request, CancellationToken cancellationToken = default)
    {
        var conversation = await _conversationRepo.GetByIdAsync(id, organizationId, cancellationToken);
        if (conversation == null) return null;

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            conversation.Title = request.Title;
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            conversation.Status = request.Status;
        }

        _conversationRepo.Update(conversation);
        await _conversationRepo.SaveChangesAsync(cancellationToken);

        return await GetConversationByIdAsync(id, organizationId, cancellationToken);
    }

    public async Task<bool> ArchiveConversationAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var conversation = await _conversationRepo.GetByIdAsync(id, organizationId, cancellationToken);
        if (conversation == null) return false;

        conversation.Status = "Archived";
        _conversationRepo.Update(conversation);
        await _conversationRepo.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteConversationAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var conversation = await _conversationRepo.GetByIdAsync(id, organizationId, cancellationToken);
        if (conversation == null) return false;

        _conversationRepo.Delete(conversation);
        await _conversationRepo.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<List<MessageDto>> GetMessageHistoryAsync(Guid conversationId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var conversation = await _conversationRepo.GetByIdAsync(conversationId, organizationId, cancellationToken);
        if (conversation == null) return new List<MessageDto>();

        var messages = await _messageRepo.GetByConversationIdAsync(conversationId, cancellationToken);
        return messages.Select(m => new MessageDto
        {
            Id = m.Id,
            ConversationId = m.ConversationId,
            Role = m.Role,
            Content = m.Content,
            ToolInvoked = m.ToolInvoked,
            ExecutionId = m.ExecutionId,
            CreatedAt = m.CreatedAt
        }).ToList();
    }
}
