using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Templates.Interfaces;

public interface ITemplateRepository
{
    Task<IEnumerable<MessageTemplate>> GetByOrganizationIdAsync(Guid organizationId, CommunicationChannelType? channelFilter, string? categoryFilter);
    Task<MessageTemplate?> GetByIdAsync(Guid id, Guid organizationId);
    Task<MessageTemplate?> GetByShortcutAsync(Guid organizationId, string shortcutCode);
    Task<MessageTemplate> CreateAsync(MessageTemplate template);
    Task<MessageTemplate> UpdateAsync(MessageTemplate template);
    Task DeleteAsync(MessageTemplate template);
}
