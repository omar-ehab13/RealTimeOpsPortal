using Microsoft.AspNetCore.Identity;
using RealTimeOpsPortal.Application.Authentication;

namespace RealTimeOpsPortal.Infrastructure.Authentication;

public sealed class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<string> _passwordHasher = new();

    public string Hash(string password)
    {
        return _passwordHasher.HashPassword(
            string.Empty,
            password);
    }

    public bool Verify(string password, string passwordHash)
    {
        var result =
            _passwordHasher.VerifyHashedPassword(
                string.Empty,
                passwordHash,
                password);

        return result is
            PasswordVerificationResult.Success or
            PasswordVerificationResult.SuccessRehashNeeded;
    }
}