using RealTimeOpsPortal.Domain.Requests;

namespace RealTimeOpsPortal.Application.Requests.Common;

internal static class ServiceRequestMappings
{
    public static ServiceRequestResult ToResult(
        this ServiceRequest request)
    {
        return new ServiceRequestResult(
            request.Id,
            request.RequestNumber,
            request.CustomerId,
            request.AssignedUserId,
            request.Description,
            request.Priority,
            request.Status,
            request.CreatedAtUtc,
            request.UpdatedAtUtc);
    }
}