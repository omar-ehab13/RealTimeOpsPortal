using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace RealTimeOpsPortal.Api.Hubs;

[Authorize]
public class OperationsHub : Hub
{
    public override Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        Console.WriteLine($"User connected: {userId}");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        Console.WriteLine($"User disconnected: {userId}");
        return base.OnDisconnectedAsync(exception);
    }
}
