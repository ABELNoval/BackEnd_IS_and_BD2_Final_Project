using AutoMapper;
using Application.DTOs.Role;
using Domain.Entities;

namespace Application.Mappers
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            // Enumeration → DTO (SOLO LECTURA)
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}