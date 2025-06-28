using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Interfaces
{
    public interface IEventService
    {
        // Task<EventResponseDto> AddEventAsync(EventCreateRequestDto dto);
        Task<EventResponseDto> AddEventWithImageAsync(EventCreateRequestDto dto, IFormFile? imageFile);
        // Task UploadEventImageAsync(EventImageUploadDto dto);
        Task<EventResponseDto> GetEventByIdAsync(Guid id);
        Task<IEnumerable<EventResponseDto>> GetAllEventsAsync();
        Task<IEnumerable<EventResponseDto>> GetUpcomingEventsAsync();
        Task<IEnumerable<EventResponseDto>> GetEventsByCategoryAsync(Guid categoryId);
        Task DeletEventAsync(Guid id);
        Task<EventResponseDto> UpdateEventAsync(Guid eventId, EventUpdateRequestDto dto);

    }
}
