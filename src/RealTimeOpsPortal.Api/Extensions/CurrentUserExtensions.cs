using RealTimeOpsPortal.Api.Authentication;
using RealTimeOpsPortal.Application.Authentication;

namespace RealTimeOpsPortal.Api.Extensions;

public static class CurrentUserExtensions
{
    public static IServiceCollection AddCurrentUserContext(this IServiceCollection services)
    {
        services.AddScoped<CurrentUserContext>();

        services.AddScoped<ICurrentUserContext>(serviceProvider =>
                serviceProvider.GetRequiredService<CurrentUserContext>());

        return services;
    }
}