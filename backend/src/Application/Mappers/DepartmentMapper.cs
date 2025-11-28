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
                .ForMember(dest => dest.SectionId, opt => opt.MapFrom(src => src.SectionId))
                .ForMember(dest => dest.ResponsibleId, opt => opt.MapFrom(src => src.ResponsibleId));

            // CreateDTO → Entity
            CreateMap<CreateDepartmentDto, Department>()
                .ConstructUsing(dto => Department.Create(
                    dto.Name,
                    dto.SectionId,
                    dto.ResponsibleId
                ));

            // UpdateDTO → Entity
            CreateMap<UpdateDepartmentDto, Department>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .AfterMap((src, dest) =>
                {
                    var sectionIdProperty = dest.GetType().GetProperty(nameof(Department.SectionId));
                    sectionIdProperty?.SetValue(dest, src.SectionId);

                    var responsibleIdProperty = dest.GetType().GetProperty(nameof(Department.ResponsibleId));
                    responsibleIdProperty?.SetValue(dest, src.ResponsibleId);
                });
        }
    }
}