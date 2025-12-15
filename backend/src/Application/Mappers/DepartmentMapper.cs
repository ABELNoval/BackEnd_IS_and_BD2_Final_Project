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

            // DTO → Entity
            CreateMap<CreateDepartmentDto, Department>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SectionId, opt => opt.MapFrom(src => src.SectionId));
        }
    }
}