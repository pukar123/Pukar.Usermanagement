using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EMS.Domain.Database;

/// <summary>
/// Design-time factory so EF CLI can run with <c>--project EMS.Domain --startup-project EMS.Domain</c>.
/// Resolves connection string from <c>EMS.API/appsettings.json</c> (sibling folder).
/// </summary>
public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var baseDir = Directory.GetCurrentDirectory();
        var apiRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "EMS.API"));
        if (!Directory.Exists(apiRoot))
            apiRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "EMS.API"));

        if (!Directory.Exists(apiRoot))
            throw new InvalidOperationException(
                $"Could not find EMS.API folder next to Domain project. Current directory: {baseDir}");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiRoot)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var migrationsAssembly = typeof(AppDbContext).Assembly.GetName().Name
            ?? throw new InvalidOperationException("Could not resolve migrations assembly name.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));

        return new AppDbContext(optionsBuilder.Options);
    }
}
