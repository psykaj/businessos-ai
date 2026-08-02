using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Channels.Interfaces;
using backend.Modules.CommunicationHub.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CommunicationHub.Channels.Repositories;

public class ChannelRepository : IChannelRepository
{
    private readonly ApplicationDbContext _context;

    public ChannelRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CommunicationChannel>> GetByOrganizationIdAsync(Guid organizationId)
    {
        return await _context.CommunicationChannels
            .Where(c => c.OrganizationId == organizationId && !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<CommunicationChannel?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.CommunicationChannels
            .FirstOrDefaultAsync(c => c.Id == id && c.OrganizationId == organizationId && !c.IsDeleted);
    }

    public async Task<CommunicationChannel?> GetByTypeAsync(Guid organizationId, CommunicationChannelType type)
    {
        return await _context.CommunicationChannels
            .FirstOrDefaultAsync(c => c.OrganizationId == organizationId && c.ChannelType == type && !c.IsDeleted);
    }

    public async Task<CommunicationChannel> CreateAsync(CommunicationChannel channel)
    {
        await _context.CommunicationChannels.AddAsync(channel);
        await _context.SaveChangesAsync();
        return channel;
    }

    public async Task<CommunicationChannel> UpdateAsync(CommunicationChannel channel)
    {
        _context.CommunicationChannels.Update(channel);
        await _context.SaveChangesAsync();
        return channel;
    }

    public async Task DeleteAsync(CommunicationChannel channel)
    {
        _context.CommunicationChannels.Remove(channel);
        await _context.SaveChangesAsync();
    }
}
