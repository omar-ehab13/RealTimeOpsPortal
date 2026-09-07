using Microsoft.EntityFrameworkCore;
using RealTimeOpsPortal.Domain.Requests;
using RealTimeOpsPortal.Domain.Users;
using RealTimeOpsPortal.Infrastructure.Persistence.Authentication;

namespace RealTimeOpsPortal.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> Users =>
        Set<ApplicationUser>();

    public DbSet<UserCredential> UserCredentials =>
    Set<UserCredential>();

    public DbSet<ServiceRequest> ServiceRequests =>
        Set<ServiceRequest>();

    public DbSet<RequestStatusHistory> RequestStatusHistories =>
        Set<RequestStatusHistory>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}