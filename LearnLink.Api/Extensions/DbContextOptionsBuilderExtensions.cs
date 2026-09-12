using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Api.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Builds <see cref="DbContextOptions"/> for SQLite using connection string from configuration.
        /// </summary>
        public static DbContextOptions GetSqliteOptions(this DbContextOptionsBuilder builder, IConfiguration config)
        {
            var connection = config.GetConnectionString("SqliteConnection");
            return builder
                .UseSqlite(connection, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))
                .Options;
        }

        /// <summary>
        /// Builds <see cref="DbContextOptions"/> for PostgreSQL using connection string from configuration.
        /// </summary>
        public static DbContextOptions GetNpgSqlOptions(this DbContextOptionsBuilder builder, IConfiguration config)
        {
            var connection = config.GetConnectionString("PostgreSqlConnection");
            return builder
                .UseNpgsql(connection, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))
                .Options;
        }

        /// <summary>
        /// Builds <see cref="DbContextOptions"/> for SQL Server using connection string from configuration.
        /// </summary>
        public static DbContextOptions GetMsSqlOptions(this DbContextOptionsBuilder builder, IConfiguration config)
        {
            var connection = config.GetConnectionString("SqlServerConnection");
            return builder
                .UseSqlServer(connection, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))
                .Options;
        }
    }
}
