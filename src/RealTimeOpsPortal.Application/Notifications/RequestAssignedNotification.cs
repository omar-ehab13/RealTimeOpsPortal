namespace RealTimeOpsPortal.Application.Notifications;

public sealed record RequestAssignedNotification(
    Guid RequestId,
    string RequestNumber,
    Guid CustomerId,
    Guid AgentId,
    DateTime AssignedAtUtc);