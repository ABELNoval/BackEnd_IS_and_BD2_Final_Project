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
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));

            // CreateDTO → Entity
            CreateMap<CreateResponsibleDto, Responsible>()
                .ConstructUsing(dto => Responsible.Create(
                    dto.Name,
                    Email.Create(dto.Email),
                    PasswordHash.CreateFromPlainText(dto.Password)
                ));

            // UpdateDTO → Entity
            CreateMap<UpdateResponsibleDto, Responsible>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .AfterMap((src, dest) =>
                {
                    var email = Email.Create(src.Email);
                    var emailProperty = dest.GetType().GetProperty("Email");
                    emailProperty?.SetValue(dest, email);
                });
        }
    }
}