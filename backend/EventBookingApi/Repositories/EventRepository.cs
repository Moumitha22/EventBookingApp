using Microsoft.EntityFrameworkCore;
using EventBookingApi.Contexts;
using EventBookingApi.Models;
using EventBookingApi.Interfaces;
using EventBookingApi.Exceptions;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Repositories
{

    public class EventRepository : Repository<Guid, Event>, IEventRepository
    {
        public EventRepository(EventBookingDbContext context) : base(context) { }

        public override async Task<Event> Get(Guid id)
        {
            var ev = await _eventBookingDbContext.Events
                .Include(e => e.Category)
                .Include(e => e.Location)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            return ev ?? throw new NotFoundException($"Event with id '{id}' not found");
        }

        public override async Task<IEnumerable<Event>> GetAll()
        {
            return await _eventBookingDbContext.Events
                .Where(e => !e.IsDeleted)
                .Include(e => e.Category)
                .Include(e => e.Location)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
        {
            return await _eventBookingDbContext.Events
                .Where(e => !e.IsDeleted && e.DateTime > DateTime.UtcNow)
                .Include(e => e.Category)
                .Include(e => e.Location)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _eventBookingDbContext.Events
                .Where(e => !e.IsDeleted && e.CategoryId == categoryId)
                .Include(e => e.Category)
                .Include(e => e.Location)
                .ToListAsync();
        }

        public Task SaveChangesAsync()
        {
            return _eventBookingDbContext.SaveChangesAsync();
        }

    }

    
}