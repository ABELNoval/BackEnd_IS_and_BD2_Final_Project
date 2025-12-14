using AutoMapper;
using Application.DTOs.Section;
using Domain.Entities;

namespace Application.Mappers
{
    public class SectionMapper : Profile
    {
        public SectionMapper()
        {
            // Entity → DTO
            CreateMap<Section, SectionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        
            // DTO → Entity
            CreateMap<CreateSectionDto, Section>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}