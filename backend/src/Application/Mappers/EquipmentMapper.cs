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

            // CreateDTO → Entity
            CreateMap<CreateEquipmentDto, Equipment>()
                .ConstructUsing(dto => Equipment.Create(
                    dto.Name,
                    dto.AcquisitionDate,
                    dto.EquipmentTypeId,
                    dto.DepartmentId,
                    dto.StateId,
                    dto.LocationTypeId
                ));

            // UpdateDTO → Entity
            CreateMap<UpdateEquipmentDto, Equipment>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .AfterMap((src, dest) =>
                {

                    var acquisitionDateProperty = dest.GetType().GetProperty("AcquisitionDate");
                    acquisitionDateProperty?.SetValue(dest, src.AcquisitionDate);

                    var equipmentTypeIdProperty = dest.GetType().GetProperty("EquipmentTypeId");
                    equipmentTypeIdProperty?.SetValue(dest, src.EquipmentTypeId);

                    var departmentIdProperty = dest.GetType().GetProperty("DepartmentId");
                    departmentIdProperty?.SetValue(dest, src.DepartmentId);
                });
        }
    }
}