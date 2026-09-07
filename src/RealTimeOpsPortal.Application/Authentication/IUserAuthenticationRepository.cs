using RealTimeOpsPortal.Domain.Users;

namespace RealTimeOpsPortal.Application.Authentication;

public interface IUserAuthenticationRepository
{
    Task<ApplicationUser?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<string?> GetPasswordHashAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}