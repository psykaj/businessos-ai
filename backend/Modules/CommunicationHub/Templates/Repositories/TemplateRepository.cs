using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Templates.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CommunicationHub.Templates.Repositories;

public class TemplateRepository : ITemplateRepository
{
    private readonly ApplicationDbContext _context;

    public TemplateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MessageTemplate>> GetByOrganizationIdAsync(Guid organizationId, CommunicationChannelType? channelFilter, string? categoryFilter)
    {
        var query = _context.CommMessageTemplates
            .Where(t => t.OrganizationId == organizationId && !t.IsDeleted);

        if (channelFilter.HasValue)
        {
            query = query.Where(t => t.ChannelType == null || t.ChannelType == channelFilter.Value);
        }

        if (!string.IsNullOrWhiteSpace(categoryFilter))
        {
            query = query.Where(t => t.Category.ToLower() == categoryFilter.ToLower());
        }

        return await query.OrderByDescending(t => t.UsageCount).ThenBy(t => t.Title).ToListAsync();
    }

    public async Task<MessageTemplate?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.CommMessageTemplates
            .FirstOrDefaultAsync(t => t.Id == id && t.OrganizationId == organizationId && !t.IsDeleted);
    }

    public async Task<MessageTemplate?> GetByShortcutAsync(Guid organizationId, string shortcutCode)
    {
        return await _context.CommMessageTemplates
            .FirstOrDefaultAsync(t => t.OrganizationId == organizationId && t.ShortcutCode.ToLower() == shortcutCode.ToLower() && !t.IsDeleted);
    }

    public async Task<MessageTemplate> CreateAsync(MessageTemplate template)
    {
        await _context.CommMessageTemplates.AddAsync(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task<MessageTemplate> UpdateAsync(MessageTemplate template)
    {
        _context.CommMessageTemplates.Update(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task DeleteAsync(MessageTemplate template)
    {
        _context.CommMessageTemplates.Remove(template);
        await _context.SaveChangesAsync();
    }
}
