using AutoMapper;
using Application.DTOs.EquipmentType;
using Domain.Entities;

namespace Application.Mappers
{
    public class EquipmentTypeMapper : Profile
    {
        public EquipmentTypeMapper()
        {
            // Entity → DTO
            CreateMap<EquipmentType, EquipmentTypeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));     
        
        }
    }
}