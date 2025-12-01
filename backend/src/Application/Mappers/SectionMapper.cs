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
               
            // CreateDTO → Entity
            CreateMap<CreateSectionDto, Section>()
                .ConstructUsing(dto => Section.Create(dto.Name));

            // UpdateDTO → Entity
            CreateMap<UpdateSectionDto, Section>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .AfterMap((src, dest) =>
                {
                    var nameProperty = dest.GetType().GetProperty("Name");
                    nameProperty?.SetValue(dest, src.Name);
                });
        }
    }
}