using EventBookingApi.Interfaces;
using EventBookingApi.Models;

namespace EventBookingApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            return await _userRepository.Add(user);
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.Get(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            email = email.Trim().ToLowerInvariant();
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAll();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.Get(id);
            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.SaveChangesAsync(); 
        }
    }
}
