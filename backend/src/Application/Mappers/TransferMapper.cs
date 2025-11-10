using AutoMapper;
using Application.DTOs.Transfer;
using Domain.Entities;

namespace Application.Mappers
{
    public class TransferMapper : Profile
    {
        public TransferMapper()
        {
            // Entity → DTO
            CreateMap<Transfer, TransferDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EquipmentId, opt => opt.MapFrom(src => src.EquipmentId))
                .ForMember(dest => dest.SourceDepartmentId, opt => opt.MapFrom(src => src.SourceDepartmentId))
                .ForMember(dest => dest.TargetDepartmentId, opt => opt.MapFrom(src => src.TargetDepartmentId))
                .ForMember(dest => dest.ResponsibleId, opt => opt.MapFrom(src => src.ResponsibleId))
                .ForMember(dest => dest.TransferDate, opt => opt.MapFrom(src => src.TransferDate))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            // CreateDTO → Entity
            CreateMap<CreateTransferDto, Transfer>()
                .ConstructUsing(dto => Transfer.Create(
                    dto.EquipmentId,
                    dto.SourceDepartmentId,
                    dto.TargetDepartmentId,
                    dto.ResponsibleId,
                    dto.TransferDate
                ));

            // UpdateDTO → Entity
            CreateMap<UpdateTransferDto, Transfer>()
                .ForMember(dest => dest.TransferDate, opt => opt.MapFrom(src => src.TransferDate))
                .AfterMap((src, dest) =>
                {
                    // Si cambia el departamento destino
                    if (src.TargetDepartmentId != Guid.Empty)
                    {
                        var prop = dest.GetType().GetProperty("TargetDepartmentId");
                        prop?.SetValue(dest, src.TargetDepartmentId);
                    }
                });
        }
    }
}
