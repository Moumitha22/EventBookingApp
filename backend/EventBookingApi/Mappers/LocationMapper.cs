using AutoMapper;
using EventBookingApi.Models;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Mappers
{
    public class LocationMapper : Profile
    {
        public LocationMapper()
        {
            CreateMap<LocationCreateRequestDto, Location>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
        }
    }
}
