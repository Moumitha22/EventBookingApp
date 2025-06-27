using EventBookingApi.Models;

namespace EventBookingApi.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<Guid, RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
    }
}
