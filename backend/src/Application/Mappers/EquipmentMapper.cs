using AutoMapper;
using Application.DTOs.Equipment;
using Domain.Entities;
using Domain.Enumerations;

namespace Application.Mappers
{
    public class EquipmentMapper : Profile
    {
        public EquipmentMapper()
        {
            // Entity → DTO
            CreateMap<Equipment, EquipmentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.AcquisitionDate, opt => opt.MapFrom(src => src.AcquisitionDate))
                .ForMember(dest => dest.EquipmentTypeId, opt => opt.MapFrom(src => src.EquipmentTypeId))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.StateId))          
                .ForMember(dest => dest.LocationTypeId, opt => opt.MapFrom(src => src.LocationTypeId));
        }
    }
}