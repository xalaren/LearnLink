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
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var rootDirectory = builder.Environment.ContentRootPath;

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
builder.Services.AddScoped<UserInteractor>();
builder.Services.AddScoped<CourseInteractor>();
builder.Services.AddScoped<SubscriptionInteractor>();
builder.Services.AddScoped<ModuleInteractor>();
builder.Services.AddScoped<LessonInteractor>();
builder.Services.AddScoped<UserVerifierService>();
builder.Services.AddScoped<RoleInteractor>();
builder.Services.AddScoped<LocalRoleInteractor>();
builder.Services.AddScoped<CourseLocalRoleInteractor>();
builder.Services.AddScoped<UserCourseLocalRolesInteractor>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<CompletionInteractor>();
builder.Services.AddScoped<ContentInteractor>();
builder.Services.AddScoped<SectionInteractor>();


builder.Services.AddTransient<SeedData>();

builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

builder.Services.AddSingleton(provider => new AuthenticationConfig().GetAuthenticationOptions(configuration));
builder.Services.AddTransient(provider => new DirectoryStore(rootDirectory));
builder.Services.AddSingleton<RegisterConfig>();

builder.Services.AddDbContext<AppDbContext>(options => options.GetNpgSqlOptions(configuration));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRazorPages();


/* Setup authentication start */

var authOptions = new AuthenticationConfig().GetAuthenticationOptions(configuration);

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
    }
);

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

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllers();
app.MapRazorPages();

app.UseInternalStorage();

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(new DirectoryStore(rootDirectory).InternalStorageDirectory)),
    RequestPath = "/api/" + DirectoryStore.STORAGE_DIRNAME
});

app.UseSeedData();

app.Run();