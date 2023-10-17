using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Adapter.EFRepositories;
using CoursesPrototype.Adapter.EFTransaction;
using CoursesPrototype.Application.Interactors;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Application.Security;
using CoursesPrototype.Application.Transaction;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.SecurityProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICredentialsRepository, CredentialsRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<AuthenticationOptions>(provider => GetAuthenticationOptions(configuration));


            //builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(sqliteConnection));
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlServerConnection));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            /* Setup authentication start */

            var authOptions = GetAuthenticationOptions(configuration);

            builder.Services.AddAuthorization();
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = authOptions.Audience,

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                    };
                });

            /* Setup authentication end */

            builder.Services.AddAuthorization();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "CoursesPrototype", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });

            });

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

        public static AuthenticationOptions GetAuthenticationOptions(IConfiguration config)
        {
            return config.GetSection("AuthenticationOptions").Get<AuthenticationOptions>()!;
        }
    }
}