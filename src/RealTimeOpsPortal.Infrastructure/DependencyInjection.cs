using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealTimeOpsPortal.Application.Authentication;
using RealTimeOpsPortal.Infrastructure.Authentication;
using RealTimeOpsPortal.Infrastructure.Persistence;

namespace RealTimeOpsPortal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<
            IUserAuthenticationRepository,
            UserAuthenticationRepository>();

        services.AddSingleton<
            IPasswordHasher,
            PasswordHasherService>();

        services.AddSingleton<
            ITokenService,
            JwtTokenService>();

        return services;
    }
}