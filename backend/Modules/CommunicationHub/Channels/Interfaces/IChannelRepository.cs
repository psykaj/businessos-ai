using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Channels.Interfaces;

public interface IChannelRepository
{
    Task<IEnumerable<CommunicationChannel>> GetByOrganizationIdAsync(Guid organizationId);
    Task<CommunicationChannel?> GetByIdAsync(Guid id, Guid organizationId);
    Task<CommunicationChannel?> GetByTypeAsync(Guid organizationId, CommunicationChannelType type);
    Task<CommunicationChannel> CreateAsync(CommunicationChannel channel);
    Task<CommunicationChannel> UpdateAsync(CommunicationChannel channel);
    Task DeleteAsync(CommunicationChannel channel);
}
