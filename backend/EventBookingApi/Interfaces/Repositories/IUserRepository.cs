using EventBookingApi.Models;

namespace EventBookingApi.Interfaces
{
    public interface IUserRepository : IRepository<Guid, User>
    {
        Task<User?> GetByEmailAsync(string email);

    }

}