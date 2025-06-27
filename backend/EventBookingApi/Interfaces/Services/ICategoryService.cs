using EventBookingApi.Models;

namespace EventBookingApi.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(Guid id);
        Task<Category?> GetByNameAsync(string name);
    }
}
