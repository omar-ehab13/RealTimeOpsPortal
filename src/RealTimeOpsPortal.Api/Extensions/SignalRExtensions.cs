using Microsoft.AspNetCore.SignalR;
using RealTimeOpsPortal.Api.SignalR;
using RealTimeOpsPortal.Application.Notifications;

namespace RealTimeOpsPortal.Api.Extensions;

public static class SignalRExtensions
{
    public static IServiceCollection AddPortalSignalR(this IServiceCollection services)
    {
        services.AddSignalR();

        services.AddSingleton<IUserIdProvider, UserIdProvider>();
        services.AddSingleton<IRequestNotificationService, SignalRRequestNotificationService>();

        return services;
    }
}
