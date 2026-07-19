using backend.Entities;
using backend.Modules.Notifications.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Notifications.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Notification?> GetByIdAsync(Guid organizationId, Guid id)
    {
        return await _context.Notifications
            .FirstOrDefaultAsync(n => n.OrganizationId == organizationId && n.Id == id);
    }

    public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid organizationId, string userId, int limit = 50)
    {
        return await _context.Notifications
            .Where(n => n.OrganizationId == organizationId && n.UserId == userId && !n.IsDeleted)
            .OrderByDescending(n => n.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(Guid organizationId, string userId)
    {
        return await _context.Notifications
            .Where(n => n.OrganizationId == organizationId && n.UserId == userId && n.Status == "Unread" && !n.IsDeleted)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<Notification> AddAsync(Notification notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task UpdateAsync(Notification notification)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync();
    }

    public async Task MarkAllAsReadAsync(Guid organizationId, string userId)
    {
        var unread = await _context.Notifications
            .Where(n => n.OrganizationId == organizationId && n.UserId == userId && n.Status == "Unread")
            .ToListAsync();
            
        foreach (var notification in unread)
        {
            notification.Status = "Read";
            notification.ReadAt = DateTime.UtcNow;
            notification.UpdatedAt = DateTime.UtcNow;
        }
        
        await _context.SaveChangesAsync();
    }
}
