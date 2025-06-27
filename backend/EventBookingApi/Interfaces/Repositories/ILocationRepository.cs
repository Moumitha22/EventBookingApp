using EventBookingApi.Models;

namespace EventBookingApi.Interfaces
{
    public interface ILocationRepository : IRepository<Guid, Location>
    {
        Task<Location?> GetByNameAndCityAsync(string name, string city);
    }
}
