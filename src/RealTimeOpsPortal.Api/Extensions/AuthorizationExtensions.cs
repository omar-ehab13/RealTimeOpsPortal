using Microsoft.AspNetCore.Authorization;
using RealTimeOpsPortal.Api.Authorization;
using RealTimeOpsPortal.Api.Authorization.Requests;
using RealTimeOpsPortal.Application.Authorization;
using RealTimeOpsPortal.Application.Authorization.Requests;

namespace RealTimeOpsPortal.Api.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddPortalAuthorization(
        this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(AuthorizationPolicyNames.CanAccessRequest, policy =>
                {
                    policy.Requirements.Add(
                        new CanAccessRequestRequirement());
                });

        services.AddScoped<
            IAuthorizationHandler,
            CanAccessRequestHandler>();

        return services;
    }
}