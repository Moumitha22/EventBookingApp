using EventBookingApi.Models;

namespace EventBookingApi.Interfaces
{
    public interface IEventRepository : IRepository<Guid, Event>
    {
        Task<IEnumerable<Event>> GetUpcomingEventsAsync();
        Task<IEnumerable<Event>> GetByCategoryIdAsync(Guid categoryId);
    }
    
}