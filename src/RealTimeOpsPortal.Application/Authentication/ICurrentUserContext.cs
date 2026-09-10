using RealTimeOpsPortal.Domain.Users;

namespace RealTimeOpsPortal.Application.Authentication;

public interface ICurrentUserContext
{
    bool IsAuthenticated { get; }

    Guid? UserId { get; }

    string? Email { get; }

    string? DisplayName { get; }

    UserRole? Role { get; }
}