using LogisticsSystem.Abstraction.Services;
using LogisticsSystem.Persistence.Contexts;
using LogisticsSystem.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LogisticsSystem.Service.Services;

public class NotificationService(ApplicationDbContext context) : INotificationService
{
    public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId)
    {
        return await context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Body = n.Body,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead,
                UserId = n.UserId
            })
            .ToListAsync();
    }

    public async Task<bool> MarkAsReadAsync(int notificationId)
    {
        var notification = await context.Notifications.FindAsync(notificationId);
        if (notification is null)
            return false;

        notification.IsRead = true;
        await context.SaveChangesAsync();
        return true;
    }
}