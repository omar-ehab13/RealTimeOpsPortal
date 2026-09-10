using Microsoft.Extensions.DependencyInjection;
using RealTimeOpsPortal.Application.Authentication;
using RealTimeOpsPortal.Application.Requests.Create;
using RealTimeOpsPortal.Application.Requests.GetById;
using RealTimeOpsPortal.Application.Requests.GetMy;

namespace RealTimeOpsPortal.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<LoginUseCase>();
        services.AddScoped<CreateServiceRequestUseCase>();
        services.AddScoped<GetMyServiceRequestsUseCase>();
        services.AddScoped<GetServiceRequestByIdUseCase>();

        return services;
    }
}