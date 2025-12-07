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
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ResponsibleId, opt => opt.MapFrom(src => src.ResponsibleId));
               
            // CreateDTO → Entity
            CreateMap<CreateSectionDto, Section>()
                .ConstructUsing(dto => Section.Create(dto.Name, dto.ResponsibleId));

            // UpdateDTO → Entity
            CreateMap<UpdateSectionDto, Section>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .AfterMap((src, dest) =>
                {
                    var nameProperty = dest.GetType().GetProperty("Name");
                    nameProperty?.SetValue(dest, src.Name);

                    // ResponsibleId has private setter - set by reflection
                    var responsibleProperty = dest.GetType().GetProperty(nameof(Section.ResponsibleId));
                    responsibleProperty?.SetValue(dest, src.ResponsibleId);
                });
        }
    }
}