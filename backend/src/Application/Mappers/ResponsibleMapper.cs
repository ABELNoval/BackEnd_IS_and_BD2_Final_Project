using AutoMapper;
using Application.DTOs.Responsible;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Mappers
{
    public class ResponsibleMapper : Profile
    {
        public ResponsibleMapper()
        {
            // Entity → DTO
            CreateMap<Responsible, ResponsibleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));
       
            // DTO → Entity
            CreateMap<CreateResponsibleDto, Responsible>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));
        }
    }
}