using Microsoft.EntityFrameworkCore;
using RealTimeOpsPortal.Application.Authentication;
using RealTimeOpsPortal.Domain.Users;
using RealTimeOpsPortal.Infrastructure.Persistence.Authentication;

namespace RealTimeOpsPortal.Infrastructure.Persistence.Seed;

public sealed class DevelopmentDataSeeder
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public DevelopmentDataSeeder(
        AppDbContext context,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync(
        CancellationToken cancellationToken = default)
    {
        if (await _context.Users.AnyAsync(cancellationToken))
            return;

        var users = new[]
        {
            new ApplicationUser(
                DevelopmentSeedConstants.Customers.Email,
                DevelopmentSeedConstants.Customers.DisplayName,
                UserRole.Customer),

            new ApplicationUser(
                DevelopmentSeedConstants.Agents.Email,
                DevelopmentSeedConstants.Agents.DisplayName,
                UserRole.Agent),

            new ApplicationUser(
                DevelopmentSeedConstants.Operations.Email,
                DevelopmentSeedConstants.Operations.DisplayName,
                UserRole.Operations),

            new ApplicationUser(
                DevelopmentSeedConstants.Supervisors.Email,
                DevelopmentSeedConstants.Supervisors.DisplayName,
                UserRole.Supervisor),

            new ApplicationUser(
                DevelopmentSeedConstants.Admins.Email,
                DevelopmentSeedConstants.Admins.DisplayName,
                UserRole.Admin)
        };

        await _context.Users.AddRangeAsync(
            users,
            cancellationToken);

        foreach (var user in users)
        {
            var credential = new UserCredential
            {
                UserId = user.Id,

                PasswordHash = _passwordHasher.Hash(
                    DevelopmentSeedConstants.DefaultPassword)
            };

            await _context.UserCredentials.AddAsync(
                credential,
                cancellationToken);
        }

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}