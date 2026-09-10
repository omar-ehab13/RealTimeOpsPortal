using RealTimeOpsPortal.Domain.Requests;

namespace RealTimeOpsPortal.Application.Requests.Create;

public sealed record CreateServiceRequestResult(
    Guid Id,
    string RequestNumber,
    string Description,
    RequestPriority Priority,
    RequestStatus Status,
    DateTime CreatedAtUtc);