namespace RealTimeOpsPortal.Infrastructure.Persistence.Authentication;

public class UserCredential
{
    public Guid UserId { get; set; }

    public string PasswordHash { get; set; } = string.Empty;
}