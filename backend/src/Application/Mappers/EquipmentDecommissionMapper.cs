using AutoMapper;
using Application.DTOs.EquipmentDecommission;
using Domain.Entities;


namespace Application.Mappers
{
    public class EquipmentDecommissionMapper : Profile
    {
        public EquipmentDecommissionMapper()
        {
            // Entity → DTO
            CreateMap<EquipmentDecommission, EquipmentDecommissionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EquipmentId, opt => opt.MapFrom(src => src.EquipmentId))
                .ForMember(dest => dest.TechnicalId, opt => opt.MapFrom(src => src.TechnicalId))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.DestinyTypeId, opt => opt.MapFrom(src => src.DestinyTypeId))
                .ForMember(dest => dest.RecipientId, opt => opt.MapFrom(src => src.RecipientId))
                .ForMember(dest => dest.DecommissionDate, opt => opt.MapFrom(src => src.DecommissionDate))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason));    
        
            // DTO → Entity
            CreateMap<CreateEquipmentDecommissionDto, EquipmentDecommission>()
                .ForMember(dest => dest.EquipmentId, opt => opt.MapFrom(src => src.EquipmentId))
                .ForMember(dest => dest.TechnicalId, opt => opt.MapFrom(src => src.TechnicalId))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.DestinyTypeId, opt => opt.MapFrom(src => src.DestinyTypeId))
                .ForMember(dest => dest.RecipientId, opt => opt.MapFrom(src => src.RecipientId))
                .ForMember(dest => dest.DecommissionDate, opt => opt.MapFrom(src => src.DecommissionDate))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason));
        }
    }
}
