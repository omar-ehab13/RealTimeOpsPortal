using Microsoft.EntityFrameworkCore;
using RealTimeOpsPortal.Domain.Requests;
using RealTimeOpsPortal.Domain.Users;

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