using EventBookingApi.Exceptions;
using EventBookingApi.Interfaces;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingResponseDto> BookEventAsync(Guid eventId, Guid userId, int seatCount)
        {
            return await _bookingRepository.BookEventAsync(eventId, userId, seatCount);
        }

        public async Task<IEnumerable<BookingResponseDto>> GetBookingsByUserIdAsync(Guid userId)
        {
            return await _bookingRepository.GetBookingsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<BookingResponseDto>> GetBookingsByEventIdAsync(Guid eventId)
        {
            return await _bookingRepository.GetBookingsByEventIdAsync(eventId);
        }

        public async Task<BookingResponseDto> GetBookingByIdAsync(Guid id)
        {
            var booking = await _bookingRepository.Get(id);
            if (booking == null)
                throw new NotFoundException($"Booking with ID '{id}' not found.");

            return new BookingResponseDto
            {
                BookingId = booking.Id,
                EventName = booking.Event.Name,
                EventDate = booking.Event.DateTime,
                SeatCount = booking.SeatCount,
                Price = booking.SeatCount * booking.Event.Price,
                BookedAt = booking.BookedAt
            };
        }

        public async Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAll();
            return bookings.Select(b => new BookingResponseDto
            {
                BookingId = b.Id,
                EventName = b.Event.Name,
                EventDate = b.Event.DateTime,
                SeatCount = b.SeatCount,
                Price = b.SeatCount * b.Event.Price,
                BookedAt = b.BookedAt
            });
        }


        public async Task<IEnumerable<EventBookingSummaryDto>> GetAllEventBookingSummariesAsync()
        {
            return await _bookingRepository.GetEventBookingSummariesAsync();
        }

    }
}
