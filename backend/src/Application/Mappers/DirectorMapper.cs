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

            
        }
    }
}