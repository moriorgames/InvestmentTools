using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EntityFrameworkContext>
{
    public EntityFrameworkContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.Development.json", optional: true)
            .AddUserSecrets<DesignTimeDbContextFactory>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var cs = configuration.GetConnectionString("InvestmentToolsDb")
                 ?? throw new InvalidOperationException("Missing ConnectionStrings:InvestmentToolsDb");

        var optionsBuilder = new DbContextOptionsBuilder<EntityFrameworkContext>();
        optionsBuilder.UseMySql(cs, ServerVersion.AutoDetect(cs));

        return new EntityFrameworkContext(optionsBuilder.Options);
    }
}