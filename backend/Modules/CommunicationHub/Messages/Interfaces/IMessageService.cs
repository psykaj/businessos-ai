using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Messages.DTOs;

namespace backend.Modules.CommunicationHub.Messages.Interfaces;

public interface IMessageService
{
    Task<IEnumerable<MessageDto>> GetMessagesAsync(Guid conversationId, Guid organizationId);
    Task<MessageDto?> SendMessageAsync(Guid organizationId, SendMessageRequest request, Guid senderUserId, string senderName);
    Task<MessageDto?> AddInternalNoteAsync(Guid organizationId, AddInternalNoteRequest request, Guid authorUserId, string authorName);
    Task<bool> MarkConversationMessagesAsReadAsync(Guid conversationId, Guid organizationId);
    Task<bool> DeleteMessageAsync(Guid id, Guid organizationId);
}
