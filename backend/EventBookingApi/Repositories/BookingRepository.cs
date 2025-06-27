using EventBookingApi.Contexts;
using EventBookingApi.Interfaces;
using EventBookingApi.Models;
using EventBookingApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using EventBookingApi.Exceptions;
using Npgsql;

namespace EventBookingApi.Repositories
{
    public class BookingRepository : Repository<Guid, Booking>, IBookingRepository
    {
        private readonly EventBookingDbContext _context;

        public BookingRepository(EventBookingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BookingResponseDto> BookEventAsync(Guid eventId, Guid userId, int seatCount)
        {
            BookingIdResult? bookingIdResult;
            try
            {
                bookingIdResult = await _context
                    .Set<BookingIdResult>()
                    .FromSqlInterpolated($"SELECT * FROM book_event_return_id({eventId}, {userId}, {seatCount})")
                    .FirstOrDefaultAsync();
            }
            catch (PostgresException ex) when (ex.Message.Contains("already booked"))
            {
                throw new ConflictException("User has already booked this event.");
            }
            catch (PostgresException ex) when (ex.Message.Contains("Event not found"))
            {
                throw new NotFoundException("The specified event does not exist.");
            }

            if (bookingIdResult == null)
                throw new BadRequestException("Booking failed. Possibly not enough seats.");

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .FirstOrDefaultAsync(b => b.Id == bookingIdResult.BookingId);

            if (booking == null)
                throw new NotFoundException("Booking not found after creation.");

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
        public async Task<IEnumerable<BookingResponseDto>> GetBookingsByUserIdAsync(Guid userId)
        {
            return await _context.Bookings
                .Include(b => b.Event)
                .Where(b => b.UserId == userId && !b.IsDeleted)
                .Select(b => new BookingResponseDto
                {
                    BookingId = b.Id,
                    EventName = b.Event.Name,
                    EventDate = b.Event.DateTime,
                    SeatCount = b.SeatCount,
                    Price = b.SeatCount * b.Event.Price,
                    BookedAt = b.BookedAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingResponseDto>> GetBookingsByEventIdAsync(Guid eventId)
        {
            return await _context.Bookings
                .Include(b => b.Event)
                .Where(b => b.EventId == eventId && !b.IsDeleted)
                .Select(b => new BookingResponseDto
                {
                    BookingId = b.Id,
                    EventName = b.Event.Name,
                    EventDate = b.Event.DateTime,
                    SeatCount = b.SeatCount,
                    Price = b.SeatCount * b.Event.Price,
                    BookedAt = b.BookedAt
                })
                .ToListAsync();
        }

        public override async Task<Booking> Get(Guid id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Event)
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            return booking ?? throw new Exception($"Booking with id '{id}' not found");
        }

        public override async Task<IEnumerable<Booking>> GetAll()
        {
            return await _context.Bookings
                .Include(b => b.Event)
                .Where(b => !b.IsDeleted)
                .ToListAsync();
        }
    }
}
