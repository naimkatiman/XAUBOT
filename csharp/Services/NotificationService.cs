using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XaubotClone.Domain;
using XaubotClone.Data;

namespace XaubotClone.Services
{
    public interface INotificationService
    {
        Task<int> SendNotificationAsync(Notification notification);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<List<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false);
        Task<bool> DeleteNotificationAsync(int notificationId);
        Task<int> SendPriceAlertAsync(int userId, TradingSymbol symbol, decimal price, string message);
        Task<int> SendTradeNotificationAsync(int userId, TradingActivity activity, string message);
        Task<int> SendSystemNotificationAsync(int userId, string subject, string message);
        Task<int> SendBulkNotificationAsync(List<int> userIds, string subject, string message);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<int> SendNotificationAsync(Notification notification)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            // Validate user exists
            var user = await _userRepository.GetByIdAsync(notification.UserId);
            if (user == null)
                throw new ArgumentException($"User with ID {notification.UserId} not found");

            // Set default values if not provided
            if (notification.CreatedAt == default)
                notification.CreatedAt = DateTime.UtcNow;

            notification.IsRead = false;

            // Save to repository
            return await _notificationRepository.AddAsync(notification);
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
                throw new ArgumentException($"Notification with ID {notificationId} not found");

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;

            await _notificationRepository.UpdateAsync(notification);
            return true;
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false)
        {
            // Validate user exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found");

            // Get notifications
            var notifications = await _notificationRepository.GetByUserIdAsync(userId);
            
            // Filter if unreadOnly is true
            if (unreadOnly)
                notifications = notifications.Where(n => !n.IsRead).ToList();

            return notifications;
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
                throw new ArgumentException($"Notification with ID {notificationId} not found");

            await _notificationRepository.DeleteAsync(notificationId);
            return true;
        }

        public async Task<int> SendPriceAlertAsync(int userId, TradingSymbol symbol, decimal price, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = NotificationType.PriceAlert,
                Subject = $"{symbol} Price Alert",
                Message = message,
                RelatedSymbol = symbol,
                CreatedAt = DateTime.UtcNow
            };

            return await SendNotificationAsync(notification);
        }

        public async Task<int> SendTradeNotificationAsync(int userId, TradingActivity activity, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = NotificationType.TradeUpdate,
                Subject = $"Trade Update: {activity.Symbol} {activity.Position}",
                Message = message,
                RelatedSymbol = activity.Symbol,
                RelatedTradingActivityId = activity.Id,
                CreatedAt = DateTime.UtcNow
            };

            return await SendNotificationAsync(notification);
        }

        public async Task<int> SendSystemNotificationAsync(int userId, string subject, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = NotificationType.System,
                Subject = subject,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            return await SendNotificationAsync(notification);
        }

        public async Task<int> SendBulkNotificationAsync(List<int> userIds, string subject, string message)
        {
            int sentCount = 0;

            foreach (var userId in userIds)
            {
                try
                {
                    // Skip if user doesn't exist
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user == null || !user.IsActive)
                        continue;

                    var notification = new Notification
                    {
                        UserId = userId,
                        Type = NotificationType.System,
                        Subject = subject,
                        Message = message,
                        CreatedAt = DateTime.UtcNow
                    };

                    await SendNotificationAsync(notification);
                    sentCount++;
                }
                catch
                {
                    // Continue with next user even if this one fails
                    continue;
                }
            }

            return sentCount;
        }
    }
}
