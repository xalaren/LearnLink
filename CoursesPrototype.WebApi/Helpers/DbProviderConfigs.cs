using CoursesPrototype.Adapter.EFContexts;
using Microsoft.EntityFrameworkCore;

namespace CoursesPrototype.WebApi.Helpers
{
    public static class DbProviderConfigs
    {
        public static DbContextOptions GetMySqlOptions(this DbContextOptionsBuilder builder, IConfiguration config)
        {
            var connection = config.GetConnectionString("MySqlConnection");

            return builder
                .UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 34)), b => b.MigrationsAssembly("CoursesPrototype.WebApi"))
                .Options;
        }

        public static DbContextOptions GetSqlServerOptions(this DbContextOptionsBuilder builder, IConfiguration config)
        {
            var connection = config.GetConnectionString("SqlServerConnection");
            return builder
                .UseSqlServer(connection, b => b.MigrationsAssembly("CoursesPrototype.WebApi"))
                .Options;
        }

        public static DbContextOptions GetSqliteOptions(this DbContextOptionsBuilder builder, IConfiguration config)
        {
            var connection = config.GetConnectionString("SqliteConnection");
            return builder
                .UseSqlite(connection, b => b.MigrationsAssembly("CoursesPrototype.WebApi"))
                .Options;
        }


    }
}
