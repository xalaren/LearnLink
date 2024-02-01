using Microsoft.EntityFrameworkCore;

namespace LearnLink.WebApi.Helpers
{
    public static class DbProviderConfigs
    {
        public static DbContextOptions GetMySqlOptions(this DbContextOptionsBuilder builder, IConfiguration config)
        {
            var connection = config.GetConnectionString("MySqlConnection");

            return builder
                .UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 35)), b => b.MigrationsAssembly("LearnLink.WebApi"))
                .Options;
        }

        public static DbContextOptions GetSqliteOptions(this DbContextOptionsBuilder builder, IConfiguration config)
        {
            var connection = config.GetConnectionString("SqliteConnection");
            return builder
                .UseSqlite(connection, b => b.MigrationsAssembly("LearnLink.WebApi"))
                .Options;
        }
    }
}
