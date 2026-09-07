using Microsoft.EntityFrameworkCore;
using RealTimeOpsPortal.Application.Authentication;
using RealTimeOpsPortal.Domain.Users;
using RealTimeOpsPortal.Infrastructure.Persistence;

namespace RealTimeOpsPortal.Infrastructure.Authentication;

public class UserAuthenticationRepository : IUserAuthenticationRepository
{
    private readonly AppDbContext _context;

    public UserAuthenticationRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);

    public Task<string?> GetPasswordHashAsync(Guid userId, CancellationToken cancellationToken = default)
        => _context.UserCredentials
            .Where(x => x.UserId == userId)
            .Select(x => x.PasswordHash)
            .FirstOrDefaultAsync(cancellationToken);
}