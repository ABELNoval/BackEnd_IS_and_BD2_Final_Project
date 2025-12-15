using AutoMapper;
using Application.DTOs.Assessment;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Mappers
{
    public class AssessmentMapper : Profile
    {
        public AssessmentMapper()
        {
            // Entity → DTO
            CreateMap<Assessment, AssessmentDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TechnicalId, opt => opt.MapFrom(src => src.TechnicalId))
                .ForMember(dest => dest.DirectorId, opt => opt.MapFrom(src => src.DirectorId))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score.Value))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.AssessmentDate, opt => opt.MapFrom(src => src.AssessmentDate));
        
            // DTO → Entity
            CreateMap<CreateAssessmentDto, Assessment>()
                .ForMember(dest => dest.TechnicalId, opt => opt.MapFrom(src => src.TechnicalId))
                .ForMember(dest => dest.DirectorId, opt => opt.MapFrom(src => src.DirectorId))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment));
        }
    }
}
