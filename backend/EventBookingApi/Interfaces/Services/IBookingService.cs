using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponseDto> BookEventAsync(Guid eventId, Guid userId, int seatCount);
        Task<IEnumerable<BookingResponseDto>> GetBookingsByUserIdAsync(Guid userId);
        Task<IEnumerable<BookingResponseDto>> GetBookingsByEventIdAsync(Guid eventId);
        Task<BookingResponseDto> GetBookingByIdAsync(Guid id);
        Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync();
    }
}
