using AutoMapper;
using Application.DTOs.User;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            // Entity → DTO (para User base - útil para listados)
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.GetRole().Name));
        }
    }
}