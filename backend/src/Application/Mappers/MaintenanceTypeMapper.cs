using AutoMapper;
using Application.DTOs.MaintenanceType;
using Domain.Enumerations;

namespace Application.Mappers
{
    public class MaintenanceTypeMapper : Profile
    {
        public MaintenanceTypeMapper()
        {
            // Enumeration → DTO (SOLO LECTURA)
            CreateMap<MaintenanceType, MaintenanceTypeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}