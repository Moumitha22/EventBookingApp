using EventBookingApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IApiResponseMapper _responseMapper;

        public BookingController(IBookingService bookingService, IApiResponseMapper responseMapper)
        {
            _bookingService = bookingService;
            _responseMapper = responseMapper;
        }

        [HttpPost("book")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BookEvent([FromQuery] Guid eventId, [FromQuery] int seatCount)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _bookingService.BookEventAsync(eventId, userId, seatCount);
            return Ok(_responseMapper.MapToOkResponse("Event booked successfully", result));
        }

        [HttpGet("my")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
            return Ok(_responseMapper.MapToOkResponse("My bookings fetched successfully", bookings));
        }

        [HttpGet("event/{eventId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBookingsByEvent(Guid eventId)
        {
            var bookings = await _bookingService.GetBookingsByEventIdAsync(eventId);
            return Ok(_responseMapper.MapToOkResponse("Bookings for event fetched successfully", bookings));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetBooking(Guid id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            return Ok(_responseMapper.MapToOkResponse("Booking fetched successfully", booking));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(_responseMapper.MapToOkResponse("All bookings fetched successfully", bookings));
        }

        [HttpGet("summary")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBookingSummary()
        {
            var summaries = await _bookingService.GetAllEventBookingSummariesAsync();
            return Ok(_responseMapper.MapToOkResponse("Booking summaries fetched successfully", summaries));
        }
    }
}
