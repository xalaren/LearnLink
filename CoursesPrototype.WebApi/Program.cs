using System.Reflection;
using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Adapter.EFRepositories;
using CoursesPrototype.Adapter.EFTransaction;
using CoursesPrototype.Application.Helpers;
using CoursesPrototype.Application.Interactors;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Application.Security;
using CoursesPrototype.Application.Transaction;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.SecurityProvider;
using CoursesPrototype.WebApi.Controllers;
using CoursesPrototype.WebApi.Extensions;
using CoursesPrototype.WebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
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

            builder.WebHost.UseUrls(ServerConfig.Url(configuration));

            builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
                policyBuilder =>
                {
                    policyBuilder.WithOrigins("https://localhost:5175")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                }));

            // Add services to the container.


            builder.Services.AddScoped<UserInteractor>();
            builder.Services.AddScoped<CourseInteractor>();
            builder.Services.AddScoped<SubscriptionInteractor>();
            builder.Services.AddScoped<ModuleInteractor>();
            builder.Services.AddScoped<UserVerifierService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICredentialsRepository, CredentialsRepository>();
            builder.Services.AddScoped<ICourseRepository, CoursesRepository>();
            builder.Services.AddScoped<IUserCreatedCoursesRepository, UserCreatedCoursesRepository>();
            builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            builder.Services.AddScoped<IModulesRepository, ModulesRepository>();
            builder.Services.AddScoped<ICourseModuleRepository, CourseModuleRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddTransient<SeedData>();

            builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton(provider => AuthenticationConfig.GetAuthenticationOptions(configuration));


            builder.Services.AddDbContext<AppDbContext>(options => options.GetMySqlOptions(configuration));

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            /* Setup authentication start */

            var authOptions = AuthenticationConfig.GetAuthenticationOptions(configuration);

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

            builder.Services.AddSwaggerGen(options =>
            {

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "CoursesPrototype WebApi", Version = "v1" });

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
                app.UseSwaggerUI(config =>
                {
                    config.RoutePrefix = string.Empty;
                    config.SwaggerEndpoint("swagger/v1/swagger.json", "CoursesPrototype WebApi");
                    config.DocumentTitle = "CoursesPrototype WebApi";
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthorization();
            app.MapControllers();


            app.UseSeedData();

            app.Run();
        }
    }
}