using Microsoft.AspNetCore.Authorization;
using RealTimeOpsPortal.Application.Authentication;
using RealTimeOpsPortal.Application.Authorization;
using RealTimeOpsPortal.Application.Authorization.Requests;
using RealTimeOpsPortal.Application.Requests.Common;

namespace RealTimeOpsPortal.Api.Authorization.Requests;

public sealed class CanAccessRequestHandler 
    : AuthorizationHandler<CanAccessRequestRequirement, ServiceRequestResult>
{
    private readonly ICurrentUserContext _currentUser;

    public CanAccessRequestHandler(
        ICurrentUserContext currentUser)
    {
        _currentUser = currentUser;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CanAccessRequestRequirement requirement,
        ServiceRequestResult resource)
    {
        if (!_currentUser.UserId.HasValue ||
            !_currentUser.Role.HasValue)
        {
            return Task.CompletedTask;
        }

        var canAccess =
            RequestAccessPolicy.CanView(
                _currentUser.UserId.Value,
                _currentUser.Role.Value,
                resource.CustomerId,
                resource.AssignedUserId);

        if (canAccess)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}