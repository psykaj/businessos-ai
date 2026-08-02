using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Messages.Interfaces;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetByConversationIdAsync(Guid conversationId, Guid organizationId);
    Task<Message?> GetByIdAsync(Guid id, Guid organizationId);
    Task<Message> CreateAsync(Message message);
    Task<IEnumerable<Message>> GetUnreadInboundMessagesAsync(Guid conversationId, Guid organizationId);
    Task UpdateRangeAsync(IEnumerable<Message> messages);
    Task DeleteAsync(Message message);
}
