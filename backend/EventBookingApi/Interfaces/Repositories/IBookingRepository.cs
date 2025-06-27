using EventBookingApi.Interfaces;
using EventBookingApi.Models;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Interfaces
{
    public interface IBookingRepository : IRepository<Guid,Booking>
    {
        Task<BookingResponseDto> BookEventAsync(Guid eventId, Guid userId, int seatCount);
        Task<IEnumerable<BookingResponseDto>> GetBookingsByUserIdAsync(Guid userId);
        Task<IEnumerable<BookingResponseDto>> GetBookingsByEventIdAsync(Guid eventId);
    }
  
}