using EventBookingApi.Models;

namespace EventBookingApi.Interfaces
{
    public interface ICategoryRepository : IRepository<Guid, Category>
    {
        Task<Category?> GetByNameAsync(string name);
    }
}
