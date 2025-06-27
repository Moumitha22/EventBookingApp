using Microsoft.EntityFrameworkCore;
using EventBookingApi.Models;
using EventBookingApi.Contexts;
using EventBookingApi.Exceptions;
using EventBookingApi.Interfaces;

namespace EventBookingApi.Repositories
{
    public class LocationRepository : Repository<Guid, Location>, ILocationRepository
    {
        public LocationRepository(EventBookingDbContext context) : base(context) { }

        public override async Task<Location> Get(Guid id)
        {
            var location = await _eventBookingDbContext.Locations
                .SingleOrDefaultAsync(l => l.Id == id && !l.IsDeleted);

            return location ?? throw new NotFoundException($"Location with id '{id}' not found");
        }

        public override async Task<IEnumerable<Location>> GetAll()
        {
            return await _eventBookingDbContext.Locations
                .Where(l => !l.IsDeleted)
                .ToListAsync();
        }

        public async Task<Location?> GetByNameAndCityAsync(string name, string city)
        {
            return await _eventBookingDbContext.Locations
                .FirstOrDefaultAsync(l =>
                    l.Name.ToLower() == name.ToLower() &&
                    l.City.ToLower() == city.ToLower() &&
                    !l.IsDeleted);
        }
    }
}
