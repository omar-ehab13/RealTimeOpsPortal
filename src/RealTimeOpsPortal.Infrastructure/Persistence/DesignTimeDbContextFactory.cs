using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RealTimeOpsPortal.Infrastructure.Persistence;

// Design-time factory to allow 'dotnet ef' tools to create AppDbContext
// without relying on the application's host configuration (which can
// require secrets like JWT keys). This factory falls back to a local
// SQLite file-based database when no connection string is provided
// via the environment variable 'ConnectionStrings__DefaultConnection'.
internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();

        // Prefer a connection string provided via environment variable
        var envConn = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        if (!string.IsNullOrEmpty(envConn))
        {
            // Assume it's a SQL Server/other provider connection string. Try SQL Server first.
            try
            {
                builder.UseSqlServer(envConn);
                return new AppDbContext(builder.Options);
            }
            catch
            {
                // If SQL Server provider isn't available or fails, fall through to SQLite below.
            }
        }

        // Fall back to a local SQL Server (LocalDB) instance for design-time operations.
        // LocalDB is commonly available on developer machines and the project already
        // references the SqlServer provider.
        var localDbConn = "Server=(localdb)\\mssqllocaldb;Database=RealTimeOpsPortal_Migrations;Trusted_Connection=True;MultipleActiveResultSets=true";
        builder.UseSqlServer(localDbConn);

        return new AppDbContext(builder.Options);
    }
}
