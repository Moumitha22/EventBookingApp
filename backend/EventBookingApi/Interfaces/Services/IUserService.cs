using EventBookingApi.Models;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task DeleteUserAsync(Guid id);
    }
}