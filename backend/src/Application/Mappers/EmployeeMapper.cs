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

            // CreateDTO → Entity
            CreateMap<CreateEmployeeDto, Employee>()
                .ConstructUsing(dto => Employee.Create(
                    dto.Name,
                    Email.Create(dto.Email),
                    PasswordHash.Create(dto.Password),
                    dto.DepartmentId
                ));

            // UpdateDTO → Entity
            CreateMap<UpdateEmployeeDto, Employee>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .AfterMap((src, dest) =>
                {
                    // var email = Email.Create(src.Email);
                    // var emailProperty = dest.GetType().GetProperty("Email");
                    // Console.WriteLine(emailProperty?.ToString());
                    // emailProperty?.SetValue(dest, email);
                    // var password = PasswordHash.Create(src.Password);
                    // var passwordProperty = dest.Gettype().GetProperty("Password");
                    // passwordProperty?.SetValue(dest, password);
                    dest.SetDepartmentId(src.DepartmentId);
                });
        }
    }
}