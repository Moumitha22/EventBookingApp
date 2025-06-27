using AutoMapper;
using EventBookingApi.Interfaces;
using EventBookingApi.Models;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public LocationService(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<Location> AddIfNotExistsAsync(LocationCreateRequestDto dto)
        {
            var existing = await _locationRepository.GetByNameAndCityAsync(dto.Name, dto.City);
            if (existing != null) return existing;

            var location = _mapper.Map<Location>(dto);
            location.Id = Guid.NewGuid();
            location.CreatedAt = DateTime.UtcNow;
            location.UpdatedAt = DateTime.UtcNow;
            location.IsDeleted = false;

            return await _locationRepository.Add(location);
        }


        // public async Task<IEnumerable<Location>> GetAllAsync()
        // {
        //     return await _locationRepository.GetAll();
        // }

        // public async Task<Location> GetByIdAsync(Guid id)
        // {
        //     return await _locationRepository.Get(id);
        // }
    }
}
