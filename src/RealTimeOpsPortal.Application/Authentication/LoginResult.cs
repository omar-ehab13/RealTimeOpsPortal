namespace RealTimeOpsPortal.Application.Authentication;

public sealed record LoginResult(
    string AccessToken,
    Guid UserId,
    string Email,
    string DisplayName,
    string Role);