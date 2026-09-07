using RealTimeOpsPortal.Domain.Users;

namespace RealTimeOpsPortal.Application.Authentication;

public interface ITokenService
{
    string GenerateToken(ApplicationUser user);
}