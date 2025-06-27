using AutoMapper;
using EventBookingApi.Models;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserRegisterRequestDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
                
            CreateMap<User, UserLoginResponseDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore());
        }
    }
}
