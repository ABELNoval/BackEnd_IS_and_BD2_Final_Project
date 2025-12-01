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

            // CreateDTO → Entity
            CreateMap<CreateEquipmentDecommissionDto, EquipmentDecommission>()
                .ConstructUsing(dto => EquipmentDecommission.Create(
                    dto.EquipmentId,
                    dto.TechnicalId,
                    dto.DepartmentId,
                    dto.DestinyTypeId,
                    dto.RecipientId,
                    dto.DecommissionDate,
                    dto.Reason
                ));

            // UpdateDTO → Entity
            CreateMap<UpdateEquipmentDecommissionDto, EquipmentDecommission>()
                .ForMember(dest => dest.DecommissionDate, opt => opt.MapFrom(src => src.DecommissionDate))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
                .ForMember(dest => dest.EquipmentId, opt => opt.Ignore())
                .ForMember(dest => dest.TechnicalId, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentId, opt => opt.Ignore())
                .ForMember(dest => dest.DestinyTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.RecipientId, opt => opt.Ignore());        
        }
    }
}
