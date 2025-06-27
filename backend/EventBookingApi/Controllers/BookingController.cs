using EventBookingApi.Interfaces;
using EventBookingApi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventBookingApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]

    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IApiResponseMapper _responseMapper;

        public BookingController(IBookingService bookingService, IApiResponseMapper apiResponseMapper)
        {
            _bookingService = bookingService;
            _responseMapper = apiResponseMapper;
        }

        [HttpPost("book")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BookEvent([FromQuery] Guid eventId, [FromQuery] int seatCount)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _bookingService.BookEventAsync(eventId, userId, seatCount);
            return Ok(result);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
            return Ok(bookings);
        }

        [HttpGet("event/{eventId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBookingsByEvent(Guid eventId)
        {
            var bookings = await _bookingService.GetBookingsByEventIdAsync(eventId);
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetBooking(Guid id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            return Ok(booking);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("summary")]
        public async Task<ActionResult> GetBookingSummary()
        {
            var summaries = await _bookingService.GetAllEventBookingSummariesAsync();
            return Ok(_responseMapper.MapToOkResponse("All events fetched successfully", summaries));
        }

    }
}
