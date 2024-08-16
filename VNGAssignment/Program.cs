using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VNGAssignment.Entities;
using VNGAssignment.Helpers;
using VNGAssignment.Middlewares;
using VNGAssignment.Migrations;
using VNGAssignment.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
var connectionStrings = builder.Configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>();
switch (connectionStrings.DataProvider)
{
    case GlobalVariable.SqlServer:
        builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connectionStrings.ConnectionString));
        break;
    default:
        builder.Services.AddDbContext<MyDbContext>(options => options.UseInMemoryDatabase("LocalDb"));
        break;
};

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "VNG Assignment", Version = "v1" });

    option.AddSecurityDefinition("XAuth", new OpenApiSecurityScheme
    {
        Name = "xAuth",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = "XAuth",
        Description = "Enter your XAuth token",
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "XAuth"
                }
            },
            new string[] {}
        }
    });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        BearerFormat = "JWT",
        Scheme = "bearer",
        Description = "Enter your JWT token in the format **Bearer {token}**"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
builder.Services.AddAutoMapper(config => config.AddProfile<MyProfile>());

var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

switch (connectionStrings.DataProvider)
{
    case GlobalVariable.SqlServer:
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            context.Database.Migrate();
            break;
        }
    default:
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            context.Books.AddRange(
                new Book { Id = 1, Title = "How to Win Friends and Influence People", Author = "Dale Carnegie", PublishedYear = "1936" },
                new Book { Id = 2, Title = "Think and Grow Rich", Author = "Napoleon Hill", PublishedYear = "1937" },
                new Book { Id = 3, Title = "The 7 Habits of Highly Effective People", Author = "Stephen R. Covey", PublishedYear = "1989" }
            );
            context.Users.AddRange(
                new User { Id = 1, Username = "admin", Password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92" }
            );
            context.SaveChanges();
            break;
        }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<XAuthMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
