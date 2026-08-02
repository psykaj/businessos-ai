using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Conversations.DTOs;

namespace backend.Modules.CommunicationHub.Conversations.Interfaces;

public interface IConversationService
{
    Task<(IEnumerable<ConversationDto> Items, int TotalCount)> GetConversationsAsync(Guid organizationId, ConversationFilterDto filter);
    Task<ConversationDto?> GetConversationByIdAsync(Guid id, Guid organizationId);
    Task<ConversationDto> CreateConversationAsync(Guid organizationId, CreateConversationRequest request, Guid currentUserId, string currentUserName);
    Task<ConversationDto?> UpdateStatusAsync(Guid id, Guid organizationId, string newStatus);
    Task<ConversationDto?> AssignConversationAsync(Guid id, Guid organizationId, Guid assignedToUserId, string assignedToUserName, Guid assignedByUserId, string assignedByUserName, string? reason);
    Task<ConversationDto?> UpdateTagsAsync(Guid id, Guid organizationId, string tags);
    Task<bool> DeleteConversationAsync(Guid id, Guid organizationId);
    Task CheckAndMarkSlaBreachesAsync();
}
