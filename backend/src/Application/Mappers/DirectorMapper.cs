using AutoMapper;
using Application.DTOs.Director;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Mappers
{
    public class DirectorMapper : Profile
    {
        public DirectorMapper()
        {
            // Entity → DTO
            CreateMap<Director, DirectorDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.GetRole().Name));

            // CreateDTO → Entity
            CreateMap<CreateDirectorDto, Director>()
                .ConstructUsing(dto => Director.Create(
                    dto.Name,
                    Email.Create(dto.Email),
                    PasswordHash.Create(dto.Password)
                ));

            // UpdateDTO → Entity
            CreateMap<UpdateDirectorDto, Director>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .AfterMap((src, dest) =>
                {
                    // var email = Email.Create(src.Email);
                    // var emailProperty = dest.GetType().GetProperty("Email");
                    // emailProperty?.SetValue(dest, email);
                });
        }
    }
}