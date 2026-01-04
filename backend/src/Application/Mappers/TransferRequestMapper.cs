using AutoMapper;
using Application.DTOs.TransferRequest;
using Domain.Entities;

namespace Application.Mappers
{
    public class TransferRequestMapper : Profile
    {
        public TransferRequestMapper()
        {
            // Entity → DTO
            CreateMap<TransferRequest, TransferRequestDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EquipmentId, opt => opt.MapFrom(src => src.EquipmentId))
                .ForMember(dest => dest.TargetDepartmentId, opt => opt.MapFrom(src => src.TargetDepartmentId))
                .ForMember(dest => dest.RequesterId, opt => opt.MapFrom(src => src.RequesterId))
                .ForMember(dest => dest.RequestedTransferDate, opt => opt.MapFrom(src => src.RequestedTransferDate))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ResolvedAt, opt => opt.MapFrom(src => src.ResolvedAt))
                .ForMember(dest => dest.ResolverId, opt => opt.MapFrom(src => src.ResolverId));
        }
    }
}
