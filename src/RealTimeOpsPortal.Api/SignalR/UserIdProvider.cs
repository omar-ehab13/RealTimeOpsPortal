using Microsoft.AspNetCore.SignalR;
using RealTimeOpsPortal.Application.Authentication;

namespace RealTimeOpsPortal.Api.SignalR;

public sealed class UserIdProvider: IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?
            .FindFirst(ClaimNames.Subject)
            ?.Value;
    }
}