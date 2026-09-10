namespace RealTimeOpsPortal.Application.Notifications;

public interface IRequestNotificationService
{
    Task NotifyRequestAssignedAsync(
        RequestAssignedNotification notification,
        CancellationToken cancellationToken = default); 
}
