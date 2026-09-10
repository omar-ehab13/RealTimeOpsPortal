using RealTimeOpsPortal.Api.Authentication;
using RealTimeOpsPortal.Application.Authentication;
using RealTimeOpsPortal.Domain.Users;

namespace RealTimeOpsPortal.Api.Middleware;

public sealed class CurrentUserMiddleware
{
    private readonly RequestDelegate _next;

    public CurrentUserMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, CurrentUserContext currentUser)
    {
        var principal = httpContext.User;

        if (principal.Identity?.IsAuthenticated == true)
        {
            var userIdValue = principal.FindFirst(ClaimNames.Subject)?.Value;

            var email = principal.FindFirst(ClaimNames.Email)?.Value;

            var displayName = principal.FindFirst(ClaimNames.DisplayName)?.Value;

            var roleValue = principal.FindFirst(ClaimNames.Role)?.Value;

            if (Guid.TryParse(userIdValue, out var userId))
            {
                UserRole? role = null;

                if (Enum.TryParse<UserRole>(roleValue, ignoreCase: true, out var parsedRole))
                {
                    role = parsedRole;
                }

                currentUser.Set(userId, email, displayName, role);
            }
        }

        await _next(httpContext);
    }
}