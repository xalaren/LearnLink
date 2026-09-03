using LearnLink.Adapter.Contexts;
using LearnLink.Adapter.DependencyInjection;
using LearnLink.Application.DependencyInjection;
using LearnLink.SecurityProvider.DependencyInjection;
using LearnLink.Api;
using LearnLink.Api.Configurations;
using LearnLink.Api.Extensions;
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

builder.Services.AddUnitOfWork();
builder.Services.AddDbContext<AppDbContext>(options => options.GetNpgSqlOptions(configuration));

builder.Services.AddEncryption();
builder.Services.AddApplicationServices();
builder.Services.AddStorage(rootDirectory);
builder.Services.AddSingleton<DefaultSystemUserConfig>();
builder.Services.AddSingleton<UrlPrinter>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "LearnLink API", Version = "v1" });
});


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

app.UseInternalStorage();
app.UseSeedData();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");

app.MapControllers();

app.UseUrlPrinter();

app.Run();