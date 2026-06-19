using LogisticsSystem.Shared.Dtos;

namespace LogisticsSystem.Abstraction.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId);
    Task<bool> MarkAsReadAsync(int notificationId);
}