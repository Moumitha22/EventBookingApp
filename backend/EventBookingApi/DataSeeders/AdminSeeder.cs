using EventBookingApi.Contexts;
using EventBookingApi.Models;
using EventBookingApi.Interfaces;
using EventBookingApi.Models.Enums;
using Microsoft.Extensions.Options;

namespace EventBookingApi.SeedData
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EventBookingDbContext>();
            var hasher = scope.ServiceProvider.GetRequiredService<IEncryptionService>();
            var options = scope.ServiceProvider.GetRequiredService<IOptions<AdminUserOptions>>();
            var adminConfig = options.Value;

            var exists = context.Users.Any(u => u.Email == adminConfig.Email && !u.IsDeleted);
            if (!exists)
            {
                var admin = new User
                {
                    Id = Guid.NewGuid(),
                    Name = adminConfig.Name,
                    Email = adminConfig.Email.ToLower(),
                    Password = hasher.HashPassword(adminConfig.Password),
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }
        }
    }
}
