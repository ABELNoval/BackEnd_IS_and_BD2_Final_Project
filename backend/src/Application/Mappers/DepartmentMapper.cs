using AutoMapper;
using Application.DTOs.Department;
using Domain.Entities;

namespace Application.Mappers
{
    public class DepartmentMapper : Profile
    {
        public DepartmentMapper()
        {
            // Entity → DTO
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SectionId, opt => opt.MapFrom(src => src.SectionId));

            // CreateDTO → Entity
            CreateMap<CreateDepartmentDto, Department>()
                .ConstructUsing(dto => Department.Create(
                    dto.Name,
                    dto.SectionId
                ));

            // UpdateDTO → Entity
            CreateMap<UpdateDepartmentDto, Department>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .AfterMap((src, dest) =>
                {
                    // SectionId has private setter - set by reflection
                    var sectionIdProperty = dest.GetType().GetProperty(nameof(Department.SectionId));
                    sectionIdProperty?.SetValue(dest, src.SectionId);
                });
        }
    }
}