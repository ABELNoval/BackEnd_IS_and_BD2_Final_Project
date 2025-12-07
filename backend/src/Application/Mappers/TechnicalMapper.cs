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
        }
    }
}