using System;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Inbox.DTOs;

namespace backend.Modules.CommunicationHub.Inbox.Interfaces;

public interface IInboxRepository
{
    Task<InboxSummaryDto> GetSummaryAsync(Guid organizationId);
}
