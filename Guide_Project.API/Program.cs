using System.Text;
using Guide_Project.Core.Models;
using Guide_Project.Core.Repositories;
using Guide_Project.Core.Services;
using Guide_Project.Core.UnitOfWork;
using Guide_Project.Data;
using Guide_Project.Data.Repositories;
using Guide_Project.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("SqlCon"), action =>
    {
        action.MigrationsAssembly("Guide_Project.Data");
    });
});
builder.Services.AddIdentity<UserEntity, RoleEntity>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, bearerOptions =>
  {
        bearerOptions.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidAudience =  builder.Configuration["Token:Audience"],
            IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            ClockSkew = TimeSpan.Zero
        };
  });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmistratorRole", policy =>
    {
        policy.RequireRole("admin");
    });
    options.AddPolicy("RequireEditorRole", policy =>
    {
        policy.RequireRole("editor");
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
