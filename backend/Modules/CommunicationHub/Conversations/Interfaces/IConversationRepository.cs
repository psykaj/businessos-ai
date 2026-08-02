using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Conversations.DTOs;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Conversations.Interfaces;

public interface IConversationRepository
{
    Task<(IEnumerable<Conversation> Items, int TotalCount)> GetFilteredAsync(Guid organizationId, ConversationFilterDto filter);
    Task<Conversation?> GetByIdAsync(Guid id, Guid organizationId);
    Task<IEnumerable<Conversation>> GetOpenConversationsForSlaCheckAsync();
    Task<Conversation> CreateAsync(Conversation conversation);
    Task<Conversation> UpdateAsync(Conversation conversation);
    Task DeleteAsync(Conversation conversation);
}
