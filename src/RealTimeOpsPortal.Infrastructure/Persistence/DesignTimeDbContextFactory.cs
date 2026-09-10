using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RealTimeOpsPortal.Infrastructure.Persistence;

internal class DesignTimeDbContextFactory
    : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
                .SetBasePath(
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "../RealTimeOpsPortal.Api"))
                .AddJsonFile(
                    "appsettings.json",
                    optional: false)
                .AddJsonFile(
                    $"appsettings.{environment}.json",
                    optional: true)
                .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");


        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "DefaultConnection was not found.");
        }


        var optionsBuilder =
            new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
            connectionString);


        return new AppDbContext(
            optionsBuilder.Options);
    }
}