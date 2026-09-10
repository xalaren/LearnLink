using LearnLink.Adapter.Contexts;
using LearnLink.Api;
using LearnLink.Api.Configurations;
using LearnLink.Api.Extensions;
using LearnLink.Api.HostedServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using Scalar.AspNetCore;
using LearnLink.Application;
using LearnLink.Adapter;
using LearnLink.SecurityProvider;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var rootDirectory = builder.Environment.ContentRootPath;

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.WebHost.UseUrls(configuration.Url());

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    policyBuilder =>
    {
        policyBuilder.WithOrigins(configuration.ClientUrl())
               .AllowAnyHeader()
               .AllowAnyMethod();
    }
));

builder.Services.AddDbContext<AppDbContext>(options => options.GetNpgSqlOptions(configuration));
builder.Services.AddAdapter();

builder.Services.AddAuthenticationOptions(configuration);
builder.Services.AddSecurityProvider();

builder.Services.AddApplication();

builder.Services.AddTransient<DefaultSystemUserConfig>();
builder.Services.AddTransient<UrlPrinter>();
builder.Services.AddHostedService<DatabaseSeedingHostedService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiWithAuth();


builder.Services.AddAuthorization();
builder
    .Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var authOptions = configuration.GetAuthenticationOptions();
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = authOptions.SecurityKey,
            ValidIssuer = authOptions.Issuer,
            ValidAudience = authOptions.Audience,
            ClockSkew = TimeSpan.Zero,
        };
    });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("LearnLink API")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseExceptionHandling();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseUrlPrinter();

app.Run();