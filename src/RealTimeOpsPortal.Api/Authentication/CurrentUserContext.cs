using RealTimeOpsPortal.Application.Authentication;
using RealTimeOpsPortal.Domain.Users;

namespace RealTimeOpsPortal.Api.Authentication;

public sealed class CurrentUserContext : ICurrentUserContext
{
    public bool IsAuthenticated { get; private set; }

    public Guid? UserId { get; private set; }

    public string? Email { get; private set; }

    public string? DisplayName { get; private set; }

    public UserRole? Role { get; private set; }

    internal void Set(
        Guid userId,
        string? email,
        string? displayName,
        UserRole? role)
    {
        UserId = userId;
        Email = email;
        DisplayName = displayName;
        Role = role;
        IsAuthenticated = true;
    }
}