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
        private readonly IWebHostEnvironment _env;

        public EventService(
            ITransactionalService transactionalService,
            IEventRepository eventRepository,
            ILocationService locationService,
            IMapper mapper,
            IWebHostEnvironment env)
        {
            _transactionalService = transactionalService;
            _eventRepository = eventRepository;
            _locationService = locationService;
            _mapper = mapper;
            _env = env;
        }

        // public async Task<EventResponseDto> AddEventAsync(EventCreateRequestDto dto)
        // {
        //     return await _transactionalService.ExecuteInTransactionAsync(async () =>
        //     {
        //         var location = await _locationService.AddIfNotExistsAsync(dto.Location);

        //         var ev = _mapper.Map<Event>(dto);
        //         ev.Id = Guid.NewGuid();
        //         ev.LocationId = location.Id;
        //         ev.AvailableSeats = ev.TotalSeats;
        //         ev.CreatedAt = DateTime.UtcNow;
        //         ev.UpdatedAt = DateTime.UtcNow;
        //         ev.IsDeleted = false;

        //         var created = await _eventRepository.Add(ev);
        //         return _mapper.Map<EventResponseDto>(created);
        //     });
        // }

        public async Task<EventResponseDto> AddEventWithImageAsync(EventCreateRequestDto dto, IFormFile? imageFile)
        {
            var createdEvent = await _transactionalService.ExecuteInTransactionAsync(async () =>
            {
                var location = await _locationService.AddIfNotExistsAsync(dto.Location);

                var ev = _mapper.Map<Event>(dto);
                ev.Id = Guid.NewGuid();
                ev.LocationId = location.Id;
                ev.AvailableSeats = ev.TotalSeats;
                ev.CreatedAt = DateTime.UtcNow;
                ev.UpdatedAt = DateTime.UtcNow;
                ev.IsDeleted = false;

                var saved = await _eventRepository.Add(ev);
                return saved;
            });

            if (imageFile != null)
            {
                await UploadEventImageAsync(createdEvent.Id, imageFile);
            }

            return _mapper.Map<EventResponseDto>(createdEvent);
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

        // public async Task UploadEventImageAsync(EventImageUploadDto dto)
        // {
        //     var eventEntity = await _eventRepository.Get(dto.EventId);

        //     var uploadsPath = Path.Combine(_env.WebRootPath, "images", "events");
        //     if (!Directory.Exists(uploadsPath))
        //         Directory.CreateDirectory(uploadsPath);

        //     var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
        //     var filePath = Path.Combine(uploadsPath, fileName);

        //     using (var stream = new FileStream(filePath, FileMode.Create))
        //     {
        //         await dto.File.CopyToAsync(stream);
        //     }

        //     eventEntity.ImageUrl = $"/images/events/{fileName}";
        //     eventEntity.UpdatedAt = DateTime.UtcNow;

        //     await _eventRepository.Update(eventEntity.Id, eventEntity);
        // }
        private async Task UploadEventImageAsync(Guid eventId, IFormFile imageFile)
        {
            var eventEntity = await _eventRepository.Get(eventId);

            var uploadsPath = Path.Combine(_env.WebRootPath, "images", "events");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            eventEntity.ImageUrl = $"/images/events/{fileName}";
            eventEntity.UpdatedAt = DateTime.UtcNow;

            await _eventRepository.Update(eventEntity.Id, eventEntity);
        }

        public async Task<EventResponseDto> UpdateEventAsync(Guid eventId, EventUpdateRequestDto dto)
        {
            return await _transactionalService.ExecuteInTransactionAsync(async () =>
            {
                var ev = await _eventRepository.Get(eventId);

                int bookedSeats = ev.TotalSeats - ev.AvailableSeats;

                ev.Name = dto.Name;
                ev.Description = dto.Description;
                ev.DateTime = DateTime.SpecifyKind(dto.DateTime, DateTimeKind.Utc);
                ev.TotalSeats = dto.TotalSeats;
                ev.Price = dto.Price;
                ev.UpdatedAt = DateTime.UtcNow;

                ev.AvailableSeats = ev.TotalSeats - bookedSeats;
                ev.AvailableSeats = Math.Max(0, Math.Min(ev.AvailableSeats, ev.TotalSeats));


                if (dto.Location != null)
                {
                    var location = await _locationService.AddIfNotExistsAsync(dto.Location);
                    ev.LocationId = location.Id;
                }

                var updated = await _eventRepository.Update(ev.Id, ev);


                return _mapper.Map<EventResponseDto>(updated);
            });
        }



        public async Task DeletEventAsync(Guid id)
        {
            var ev = await _eventRepository.Get(id);
            ev.IsDeleted = true;
            ev.UpdatedAt = DateTime.UtcNow;
            await _eventRepository.SaveChangesAsync();
        }
        
        public async Task UpdateEventImageAsync(EventImageUploadDto dto)
        {
            var ev = await _eventRepository.Get(dto.EventId);

            var wwwRootPath = _env.WebRootPath;

            // Delete old image
            if (!string.IsNullOrWhiteSpace(ev.ImageUrl))
            {
                var oldPath = Path.Combine(wwwRootPath, ev.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            // Upload new image
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
            var relativePath = Path.Combine("uploads", "events", fileName);
            var fullPath = Path.Combine(wwwRootPath, relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            ev.ImageUrl = "/" + relativePath.Replace("\\", "/");
            ev.UpdatedAt = DateTime.UtcNow;

            await _eventRepository.Update(dto.EventId, ev);
        }


    }
}
