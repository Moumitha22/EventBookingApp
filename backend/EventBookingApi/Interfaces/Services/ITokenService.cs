using EventBookingApi.Models;

namespace EventBookingApi.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(User user);
    }
}