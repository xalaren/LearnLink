using LearnLink.Adapter.EFContexts;
using LearnLink.Adapter.EFTransaction;
using LearnLink.Application.Helpers;
using LearnLink.Application.Interactors;
using LearnLink.Application.Security;
using LearnLink.Application.Transaction;
using LearnLink.SecurityProvider;
using LearnLink.WebApi.Extensions;
using LearnLink.WebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.WebHost.UseUrls(ServerConfig.Url(configuration));

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    policyBuilder =>
    {
        policyBuilder.WithOrigins("https://localhost:5174")
               .AllowAnyHeader()
               .AllowAnyMethod();
    }));

// Add services to the container.


builder.Services.AddScoped<UserInteractor>();
builder.Services.AddScoped<CourseInteractor>();
builder.Services.AddScoped<SubscriptionInteractor>();
builder.Services.AddScoped<ModuleInteractor>();
builder.Services.AddScoped<UserVerifierService>();
builder.Services.AddScoped<RoleInteractor>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<SeedData>();

builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton(provider => AuthenticationConfig.GetAuthenticationOptions(configuration));


//builder.Services.AddDbContext<AppDbContext>(options => options.GetMySqlOptions(configuration));
//builder.Services.AddDbContext<AppDbContext>(options => options.GetNpgSqlOptions(configuration));
builder.Services.AddDbContext<AppDbContext>(options => options.GetSqliteOptions(configuration));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();

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

    options.SwaggerDoc("v1", new OpenApiInfo { Title = "LearnLink WebApi", Version = "v1" });

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

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.UseSeedData();

app.Run();