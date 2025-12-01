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

                // Mapear el nombre del tipo de mantenimiento (si está disponible)
                .ForMember(dest => dest.MaintenanceTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost));
                
            // CreateDTO → Entity
            CreateMap<CreateMaintenanceDto, Maintenance>()
                .ConstructUsing(dto => Maintenance.Create(
                    dto.EquipmentId,
                    dto.TechnicalId,
                    dto.MaintenanceDate,              
                    dto.MaintenanceTypeId,
                    dto.Cost
                ));

            // UpdateDTO → Entity
            CreateMap<UpdateMaintenanceDto, Maintenance>()
                .AfterMap((src, dest) =>
                {
                    dest.GetType().GetProperty(nameof(Maintenance.Cost))!
                        .SetValue(dest, src.Cost);
                    dest.GetType().GetProperty(nameof(Maintenance.MaintenanceDate))!
                        .SetValue(dest, src.MaintenanceDate);
                    dest.GetType().GetProperty(nameof(Maintenance.MaintenanceTypeId))!
                        .SetValue(dest, src.MaintenanceTypeId);
                });
        }
    }
}
