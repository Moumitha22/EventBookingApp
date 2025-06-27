using Microsoft.EntityFrameworkCore;
using EventBookingApi.Contexts;
using EventBookingApi.Exceptions;
using EventBookingApi.Interfaces;
using EventBookingApi.Models;

namespace EventBookingApi.Repositories
{
    public class CategoryRepository : Repository<Guid, Category>, ICategoryRepository
    {
        public CategoryRepository(EventBookingDbContext context) : base(context) { }

        public override async Task<Category> Get(Guid id)
        {
            var category = await _eventBookingDbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            
            return category ?? throw new NotFoundException($"Category with id '{id}' not found");
        }

        public override async Task<IEnumerable<Category>> GetAll()
        {
            return await _eventBookingDbContext.Categories
                .Where(c => !c.IsDeleted)
                // .OrderBy()
                .ToListAsync();
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _eventBookingDbContext.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower() && !c.IsDeleted);
        }
    }
}
