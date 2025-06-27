using Microsoft.EntityFrameworkCore;
using EventBookingApi.Models;
using EventBookingApi.Contexts;
using EventBookingApi.Exceptions;
using EventBookingApi.Interfaces;

namespace EventBookingApi.Repositories
{
    public class UserRepository : Repository<Guid, User>, IUserRepository
    {
        public UserRepository(EventBookingDbContext context) : base(context) { }

        public override async Task<User> Get(Guid id)
        {
            var user = await _eventBookingDbContext.Users.SingleOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            return user ?? throw new NotFoundException($"User with id '{id}' not found");
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await _eventBookingDbContext.Users
                .Where(u => !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _eventBookingDbContext.Users
                .SingleOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

    }
}
