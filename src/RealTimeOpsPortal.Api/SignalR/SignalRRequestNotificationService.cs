using Microsoft.AspNetCore.SignalR;
using RealTimeOpsPortal.Application.Notifications;
using RealTimeOpsPortal.Api.Hubs;

namespace RealTimeOpsPortal.Api.SignalR;

public sealed class SignalRRequestNotificationService : IRequestNotificationService
{
    private readonly IHubContext<OperationsHub> _hubContext;


    public SignalRRequestNotificationService(
        IHubContext<OperationsHub> hubContext)
    {
        _hubContext = hubContext;
    }


    public async Task NotifyRequestAssignedAsync(
        RequestAssignedNotification notification,
        CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients
            .User(notification.CustomerId.ToString())
            .SendAsync(
                "RequestAssigned",
                notification,
                cancellationToken);
    }
}