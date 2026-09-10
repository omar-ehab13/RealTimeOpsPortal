using RealTimeOpsPortal.Domain.Requests;

namespace RealTimeOpsPortal.Application.Requests.Common;

public sealed record ServiceRequestResult(
    Guid Id,
    string RequestNumber,
    Guid CustomerId,
    Guid? AssignedUserId,
    string Description,
    RequestPriority Priority,
    RequestStatus Status,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);