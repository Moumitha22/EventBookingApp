using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Interfaces
{
    public interface IEventService
    {
        Task<EventResponseDto> AddEventAsync(EventCreateRequestDto dto);
        Task UploadEventImageAsync(EventImageUploadDto dto);
        Task<EventResponseDto> GetEventByIdAsync(Guid id);
        Task<IEnumerable<EventResponseDto>> GetAllEventsAsync();
        Task<IEnumerable<EventResponseDto>> GetUpcomingEventsAsync();
        Task<IEnumerable<EventResponseDto>> GetEventsByCategoryAsync(Guid categoryId);
    }
}
