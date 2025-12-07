using AutoMapper;
using Application.DTOs.Technical;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Mappers
{
    public class TechnicalMapper : Profile
    {
        public TechnicalMapper()
        {
            // Entity → DTO
            CreateMap<Technical, TechnicalDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience))
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Specialty));

            // CreateDTO → Entity
            CreateMap<CreateTechnicalDto, Technical>()
                .ConstructUsing(dto => Technical.Create(
                    dto.Name,
                    Email.Create(dto.Email),
                    PasswordHash.Create(dto.Password),
                    dto.Experience,
                    dto.Specialty
                ));

            // UpdateDTO → Entity
            CreateMap<UpdateTechnicalDto, Technical>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience))
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Specialty))
                .AfterMap((src, dest) =>
                {
                    // Actualizar email usando reflection
                    // var email = Email.Create(src.Email);
                    // var emailProperty = dest.GetType().GetProperty("Email");
                    // emailProperty?.SetValue(dest, email);
                    // var password = PasswordHash.Create(src.Password);
                    // var passwordProperty = dest.Gettype().GetProperty("Password");
                    // passwordProperty?.SetValue(dest, password);
                });
        }
    }
}