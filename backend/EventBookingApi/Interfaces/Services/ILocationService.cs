using EventBookingApi.Models;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Interfaces
{
    public interface ILocationService
    {
        Task<Location> AddIfNotExistsAsync(LocationCreateRequestDto dto);
        // Task<IEnumerable<Location>> GetAllAsync();
        // Task<Location> GetByIdAsync(Guid id);
    }
}
