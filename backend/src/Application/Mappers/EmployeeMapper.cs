using AutoMapper;
using Application.DTOs.Employee;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Mappers
{
    public class EmployeeMapper : Profile
    {
        public EmployeeMapper()
        {
            // Entity → DTO
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.GetRole().Name))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));
        }
    }
}