using AutoMapper;
using Application.DTOs.DestinyType;
using Domain.Enumerations;

namespace Application.Mappers
{
    public class DestinyTypeMapper : Profile
    {
        public DestinyTypeMapper()
        {
            // Enumeration → DTO (SOLO LECTURA)
            CreateMap<DestinyType, DestinyTypeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}