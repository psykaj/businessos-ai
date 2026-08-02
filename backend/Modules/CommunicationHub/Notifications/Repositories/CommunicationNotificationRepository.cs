using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Notifications.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CommunicationHub.Notifications.Repositories;

public class CommunicationNotificationRepository : ICommunicationNotificationRepository
{
    private readonly ApplicationDbContext _context;

    public CommunicationNotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CommunicationNotification>> GetByUserIdAsync(Guid organizationId, Guid targetUserId, bool unreadOnly)
    {
        var query = _context.CommNotifications
            .Where(n => n.OrganizationId == organizationId && n.TargetUserId == targetUserId && !n.IsDeleted);

        if (unreadOnly)
        {
            query = query.Where(n => !n.IsRead);
        }

        return await query.OrderByDescending(n => n.CreatedAt).Take(50).ToListAsync();
    }

    public async Task<CommunicationNotification?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.CommNotifications
            .FirstOrDefaultAsync(n => n.Id == id && n.OrganizationId == organizationId && !n.IsDeleted);
    }

    public async Task<CommunicationNotification> CreateAsync(CommunicationNotification notification)
    {
        await _context.CommNotifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task UpdateAsync(CommunicationNotification notification)
    {
        _context.CommNotifications.Update(notification);
        await _context.SaveChangesAsync();
    }

    public async Task MarkAllAsReadAsync(Guid organizationId, Guid targetUserId)
    {
        var unread = await _context.CommNotifications
            .Where(n => n.OrganizationId == organizationId && n.TargetUserId == targetUserId && !n.IsRead && !n.IsDeleted)
            .ToListAsync();

        var now = DateTime.UtcNow;
        foreach (var n in unread)
        {
            n.IsRead = true;
            n.ReadAt = now;
        }
        _context.CommNotifications.UpdateRange(unread);
        await _context.SaveChangesAsync();
    }
}
