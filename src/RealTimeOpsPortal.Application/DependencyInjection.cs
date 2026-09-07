using Microsoft.Extensions.DependencyInjection;
using RealTimeOpsPortal.Application.Authentication;

namespace RealTimeOpsPortal.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<LoginUseCase>();

        return services;
    }
}