using EventBookingApi.Contexts;
using EventBookingApi.Models;

namespace EventBookingApi.SeedData
{
    public static class CategorySeeder
    {
        public static async Task SeedAsync(EventBookingDbContext context)
        {
            if (!context.Categories.Any())
            {
                var now = DateTime.UtcNow;
                var categories = new List<Category>
                {
                    new() { Id = Guid.NewGuid(), Name = "Music", CreatedAt = now, UpdatedAt = now, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), Name = "Dance", CreatedAt = now, UpdatedAt = now, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), Name = "Comedy", CreatedAt = now, UpdatedAt = now, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), Name = "Sports", CreatedAt = now, UpdatedAt = now, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), Name = "Art", CreatedAt = now, UpdatedAt = now, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), Name = "Charity", CreatedAt = now, UpdatedAt = now, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), Name = "Tech Talk", CreatedAt = now, UpdatedAt = now, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), Name = "Conference", CreatedAt = now, UpdatedAt = now, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), Name = "Workshop", CreatedAt = now, UpdatedAt = now, IsDeleted = false }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
        }
    }
}
