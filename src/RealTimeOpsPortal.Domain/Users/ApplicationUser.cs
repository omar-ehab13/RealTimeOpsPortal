namespace RealTimeOpsPortal.Domain.Users;

public class ApplicationUser
{
    public Guid Id { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public UserRole Role { get; private set; }

    public bool IsActive { get; private set; }

    private ApplicationUser()
    {
    }

    public ApplicationUser(
        string email,
        string displayName,
        UserRole role)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException(
                "Display name is required.",
                nameof(displayName));

        Id = Guid.NewGuid();
        Email = email.Trim();
        DisplayName = displayName.Trim();
        Role = role;
        IsActive = true;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
