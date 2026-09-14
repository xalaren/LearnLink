using PandaRedu.Adapter.Contexts;
using PandaRedu.Api;
using PandaRedu.Api.Configurations;
using PandaRedu.Api.Extensions;
using PandaRedu.Api.HostedServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using Scalar.AspNetCore;
using PandaRedu.Application;
using PandaRedu.Adapter;
using PandaRedu.SecurityProvider;

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
            .WithTitle("PandaRedu API")
            .WithClassicLayout()
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseRouting();
app.UseCors("CorsPolicy");

app.MapControllers();

app.UseHttpsRedirection();
app.UseExceptionHandling();

app.UseAuthentication();
app.UseAuthorization();

app.UseUrlPrinter();

app.Run();