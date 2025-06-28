using AutoMapper;
using EventBookingApi.Models;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Mappers
{
    public class EventMapper : Profile
    {
        public EventMapper()
        {
            CreateMap<EventCreateRequestDto, Event>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.DateTime, DateTimeKind.Utc))) // ✅ correct
                .ForMember(dest => dest.LocationId, opt => opt.Ignore())
                .ForMember(dest => dest.AvailableSeats, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());



            CreateMap<Event, EventResponseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => $"{src.Location.Name}, {src.Location.Locality}, {src.Location.City}, {src.Location.State}"));
        }
    }
}
