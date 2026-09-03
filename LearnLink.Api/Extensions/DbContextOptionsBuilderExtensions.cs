using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.WebApi.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptions GetSqliteOptions(this DbContextOptionsBuilder builder, IConfiguration config)
        {
            var connection = config.GetConnectionString("SqliteConnection");
            return builder
                .UseSqlite(connection, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))
                .Options;
        }

        public static DbContextOptions GetNpgSqlOptions(this DbContextOptionsBuilder builder, IConfiguration config)
        {
            var connection = config.GetConnectionString("PostgreSqlConnection");
            return builder
                .UseNpgsql(connection, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))
                .Options;
        }

        public static DbContextOptions GetMsSqlOptions(this DbContextOptionsBuilder builder, IConfiguration config)
        {
            var connection = config.GetConnectionString("SqlServerConnection");
            return builder
                .UseSqlServer(connection, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))
                .Options;
        }
    }
}
