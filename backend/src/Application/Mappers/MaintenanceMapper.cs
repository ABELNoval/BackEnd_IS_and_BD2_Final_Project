using AutoMapper;
using Application.DTOs.Maintenance;
using Domain.Entities;

namespace Application.Mappers
{
    public class MaintenanceMapper : Profile
    {
        public MaintenanceMapper()
        {
            // Entity → DTO
            CreateMap<Maintenance, MaintenanceDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EquipmentId, opt => opt.MapFrom(src => src.EquipmentId))
                .ForMember(dest => dest.TechnicalId, opt => opt.MapFrom(src => src.TechnicalId))
                .ForMember(dest => dest.MaintenanceDate, opt => opt.MapFrom(src => src.MaintenanceDate))
                .ForMember(dest => dest.MaintenanceTypeId, opt => opt.MapFrom(src => src.MaintenanceTypeId))

                .ForMember(dest => dest.MaintenanceTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost));
        }
    }
}
