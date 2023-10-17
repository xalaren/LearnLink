using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Adapter.EFRepositories;
using CoursesPrototype.Adapter.EFTransaction;
using CoursesPrototype.Application.Interactors;
using CoursesPrototype.Application.RepositoryInterfaces;
using CoursesPrototype.Application.Transaction;
using CoursesPrototype.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursePrototype.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;
            var sqliteConnection = configuration.GetConnectionString("SqliteConnection");
            var sqlServerConnection = configuration.GetConnectionString("SqlServerConnection");

            // Add services to the container.

            builder.Services.AddScoped<UserInteractor>();
            builder.Services.AddScoped<IAsyncRepository<User>, AsyncRepository<User>>();
            builder.Services.AddScoped<IAsyncRepository<Credentials>, AsyncRepository<Credentials>>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(sqliteConnection));
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlServerConnection));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}