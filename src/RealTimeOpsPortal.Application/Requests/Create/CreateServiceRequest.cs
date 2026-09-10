using RealTimeOpsPortal.Domain.Requests;

namespace RealTimeOpsPortal.Application.Requests.Create;

public sealed record CreateServiceRequest(
    string Description,
    RequestPriority Priority);