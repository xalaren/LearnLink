using LearnLink.Adapter.Contexts;
using LearnLink.Adapter.Repositories;
using LearnLink.Adapter.Transactions;
using LearnLink.Application.Repositories;
using LearnLink.Application.Security;
using LearnLink.Application.Services;
using LearnLink.Application.Storage;
using LearnLink.Application.Transactions;
using LearnLink.SecurityProvider;
using LearnLink.WebApi;
using LearnLink.WebApi.Configurations;
using LearnLink.WebApi.Extensions;
using Microsoft.OpenApi;
using NLog.Web;

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

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICredentialsRepository, CredentialsRepository>();

builder.Services.AddTransient<SeedingService>();

builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
builder.Services.AddSingleton(provider => Storage.Instance(rootDirectory));
builder.Services.AddSingleton<DefaultSystemUserConfig>();

builder.Services.AddDbContext<AppDbContext>(options => options.GetNpgSqlOptions(configuration));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "LearnLink API", Version = "v1" });
});

builder.Services.AddSingleton<UrlPrinter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");

app.MapControllers();

app.UseInternalStorage();
app.UseSeedData();

app.UseUrlPrinter();

app.Run();