using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EventBookingApi.Contexts;
using EventBookingApi.Interfaces;
using EventBookingApi.Mappers;
using EventBookingApi.Middleware;
using EventBookingApi.Models;
using EventBookingApi.Repositories;
using EventBookingApi.SeedData;
using EventBookingApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
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
                Array.Empty<string>()
            }
        });
    });

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opts.JsonSerializerOptions.WriteIndented = true;
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Datbase Context
builder.Services.AddDbContext<EventBookingDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.Configure<AdminUserOptions>(builder.Configuration.GetSection("AdminUser"));


// Repositories
#region Repositories
builder.Services.AddTransient<IRepository<Guid, User>, UserRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IEventRepository, EventRepository>();
builder.Services.AddTransient<ILocationRepository, LocationRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IBookingRepository, BookingRepository>();
#endregion

// Services
#region Services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IEncryptionService, EncryptionService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ILocationService, LocationService>();
builder.Services.AddTransient<IEventService, EventService>();
builder.Services.AddTransient<ITransactionalService, TransactionalService>();
builder.Services.AddTransient<IBookingService, BookingService>();
builder.Services.AddTransient<IAdminService, AdminService>();
#endregion

builder.Services.AddAutoMapper(typeof(UserMapper).Assembly);
builder.Services.AddTransient<IApiResponseMapper, ApiResponseMapper>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "http://127.0.0.1:5500"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// JWT Authentication & Authorization
#region Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtTokenKey"]))
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
                var email = context.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value ?? "Unknown";

                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    message = "Unauthorized",
                    data = (object)null,
                    errors = new Dictionary<string, string[]>
                    {
                        { "general", new[] { "Authentication failed or token is missing/invalid." } }
                    }
                };

                var json = JsonSerializer.Serialize(response);
                return context.Response.WriteAsync(json);
            },
            OnForbidden = context =>
            {
                var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
                var email = context.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value ?? "Unknown";
                
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    message = "Forbidden",
                    data = (object)null,
                    errors = new Dictionary<string, string[]>
                    {
                        { "general", new[] { "You do not have permission to access this." } }
                    }
                };

                var json = JsonSerializer.Serialize(response);
                return context.Response.WriteAsync(json);
            }
        };
    });
builder.Services.AddAuthorization();
#endregion


var app = builder.Build();
app.UseStaticFiles();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EventBookingDbContext>();
    await CategorySeeder.SeedAsync(context);
    await AdminSeeder.SeedAsync(scope.ServiceProvider);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
