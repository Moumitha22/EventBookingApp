using System.Text.Json;
using EventBookingApi.Interfaces;
using EventBookingApi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IApiResponseMapper _responseMapper;

        public EventController(IEventService eventService, IApiResponseMapper responseMapper)
        {
            _eventService = eventService;
            _responseMapper = responseMapper;
        }

        // [HttpPost]
        // // [Authorize(Roles = "Admin")]
        // public async Task<IActionResult> AddEvent([FromBody] EventCreateRequestDto dto)
        // {
        //     var createdEvent = await _eventService.AddEventAsync(dto);
        //     return Ok(_responseMapper.MapToOkResponse("Event created successfully", createdEvent));
        // }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadEventWithImage([FromForm] EventWithImageUploadDto dto)
        {
            var eventDto = JsonSerializer.Deserialize<EventCreateRequestDto>(dto.EventJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (eventDto == null) return BadRequest("Invalid event data");

            var result = await _eventService.AddEventWithImageAsync(eventDto, dto.Image);

            return Ok(_responseMapper.MapToOkResponse("Event created successfully", result));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var ev = await _eventService.GetEventByIdAsync(id);
            return Ok(_responseMapper.MapToOkResponse("Event fetched successfully", ev));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(_responseMapper.MapToOkResponse("All events fetched successfully", events));
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingEvents()
        {
            var events = await _eventService.GetUpcomingEventsAsync();
            return Ok(_responseMapper.MapToOkResponse("Upcoming events fetched successfully", events));
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetEventsByCategory(Guid categoryId)
        {
            var events = await _eventService.GetEventsByCategoryAsync(categoryId);
            return Ok(_responseMapper.MapToOkResponse("Events by category fetched successfully", events));
        }

        // [HttpPost("upload-image")]
        // [Consumes("multipart/form-data")]
        // public async Task<IActionResult> UploadEventImage([FromForm] EventImageUploadDto dto)
        // {
        //     await _eventService.UploadEventImageAsync(dto);
        //     return Ok(_responseMapper.MapToOkResponse("Image uploaded successfully"));
        // }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventUpdateRequestDto dto)
        {
            var updated = await _eventService.UpdateEventAsync(id, dto);
            return Ok(_responseMapper.MapToOkResponse("Events updated successfully", updated));
        }


        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            await _eventService.DeletEventAsync(id);
            return Ok(_responseMapper.MapToOkResponse("Event deleted successfully"));
        }
        
        [HttpPut("image")]
        // [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateEventImage([FromForm] EventImageUploadDto dto)
        {
            await _eventService.UpdateEventImageAsync(dto);
            return Ok(new { message = "Event image updated successfully" });
        }


    }
}
