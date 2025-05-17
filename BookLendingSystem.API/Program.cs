using BookLendingSystem.Infrastructure.Context;
using BookLendingSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookLendingSystem.Application.Interfaces.IRepositories;
using BookLendingSystem.Persistence.Repositories;
using BookLendingSystem.Application.Mappings;
using Microsoft.AspNetCore.Cors.Infrastructure;
using BookLendingSystem.Application.Interfaces.IServices;
using BookLendingSystem.Application.Services;
using BookLendingSystem.Application.Helpers;
using BookLendingSystem.Application.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookLendingSystem.Infrastructure.Seed;
using BookLendingSystem.Domain.Entities.Business;
using Hangfire;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using Serilog;
using BookLendingSystem.API;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<JWT>(builder.Configuration.GetSection(AuthConstants.JwtSection));
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IBorrowedBookRepository, BorrowedBookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();


builder.Services.AddScoped<IAuthService, JwtService>();



//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    var jwtSettings = builder.Configuration.GetSection("JWT").Get<JWT>();

//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = jwtSettings.Issuer,
//        ValidAudience = jwtSettings.Audience,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
//        ClockSkew=TimeSpan.FromSeconds(1),
//        RoleClaimType = ClaimTypes.Role
//    };
//});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>()
          ;
builder.Services.AddAuthentication(options =>
{
    //Check JWT Token Header
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //[authrize]
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//unauth
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

builder.Services.Configure<EmailSetting>(
   builder.Configuration.GetSection("Smtp"));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
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
}

    );
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});
builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration["ConnectionStrings:DefaultConnection"]));
builder.Services.AddHangfireServer();
builder.Host.UseSerilog();

var app = builder.Build();
using var scope = app.Services.CreateScope();
await DefaultRoles.SeedAsync(scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>());
using var scope2 = app.Services.CreateScope();
await SeedAdmin.SeedUsersAsync(scope2.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>());



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Hangfire
app.UseHangfireDashboard();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
using (var scope3 = app.Services.CreateScope())
{

    RecurringJob.AddOrUpdate<EmailService>("delayed books",
    job => job.SendEmailReturnAsync(),
    Cron.Daily);

}


app.Run();
