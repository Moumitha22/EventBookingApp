using AutoMapper;
using EventBookingApi.Interfaces;
using EventBookingApi.Models;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Services
{
    public class EventService : IEventService
    {
        private readonly ITransactionalService _transactionalService;
        private readonly IEventRepository _eventRepository;
        private readonly ILocationService _locationService;
        private readonly IMapper _mapper;

        public EventService(
            ITransactionalService transactionalService,
            IEventRepository eventRepository,
            ILocationService locationService,
            IMapper mapper)
        {
            _transactionalService = transactionalService;
            _eventRepository = eventRepository;
            _locationService = locationService;
            _mapper = mapper;
        }

        public async Task<EventResponseDto> AddEventAsync(EventCreateRequestDto dto)
        {
            return await _transactionalService.ExecuteInTransactionAsync(async () =>
            {
                var location = await _locationService.AddIfNotExistsAsync(dto.Location);

                var ev = _mapper.Map<Event>(dto);
                ev.Id = Guid.NewGuid();
                ev.LocationId = location.Id;
                ev.AvailableSeats = ev.TotalSeats;
                ev.CreatedAt = DateTime.UtcNow;
                ev.UpdatedAt = DateTime.UtcNow;
                ev.IsDeleted = false;

                var created = await _eventRepository.Add(ev);
                return _mapper.Map<EventResponseDto>(created);
            });
        }

        public async Task<EventResponseDto> GetEventByIdAsync(Guid id)
        {
            var ev = await _eventRepository.Get(id);
            return _mapper.Map<EventResponseDto>(ev);
        }

        public async Task<IEnumerable<EventResponseDto>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetAll();
            return events.Select(_mapper.Map<EventResponseDto>);
        }

        public async Task<IEnumerable<EventResponseDto>> GetUpcomingEventsAsync()
        {
            var events = await _eventRepository.GetUpcomingEventsAsync();
            return events.Select(_mapper.Map<EventResponseDto>);
        }

        public async Task<IEnumerable<EventResponseDto>> GetEventsByCategoryAsync(Guid categoryId)
        {
            var events = await _eventRepository.GetByCategoryIdAsync(categoryId);
            return events.Select(_mapper.Map<EventResponseDto>);
        }
    }
}
