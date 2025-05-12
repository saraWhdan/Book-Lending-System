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

var builder = WebApplication.CreateBuilder(args);

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



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection(AuthConstants.JwtSection).Get<JWT>();

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

builder.Services.Configure<EmailSetting>(
   builder.Configuration.GetSection("Smtp"));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});
builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration["ConnectionStrings:DefaultConnection"]));
builder.Services.AddHangfireServer();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();
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

app.UseAuthorization();
//Hangfire
app.UseHangfireDashboard();
app.MapControllers();
using (var scope3 = app.Services.CreateScope())
{

    RecurringJob.AddOrUpdate<EmailService>(
    job => job.SendEmailReturnAsync(),
    Cron.Daily);

}


app.Run();
