using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Infrastructure.Persistence;

public sealed class EntityFrameworkContextFactory : IDesignTimeDbContextFactory<EntityFrameworkContext>
{
    public EntityFrameworkContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EntityFrameworkContext>();
        var connectionString = "Server=localhost;Port=3306;Database=investment_tools;User Id=it_user;Password=it_password;";

        optionsBuilder.UseMySql(
            connectionString,
            ServerVersion.Create(new Version(8, 0, 39), ServerType.MySql),
            mysqlOptionsAction: builder =>
                builder.MigrationsAssembly(typeof(EntityFrameworkContext).Assembly.GetName().Name));

        return new EntityFrameworkContext(optionsBuilder.Options);
    }
}
