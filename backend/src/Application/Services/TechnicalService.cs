using Application.DTOs.Technical;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class TechnicalService : ITechnicalService
    {
        private readonly ITechnicalRepository _technicalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TechnicalService(
            ITechnicalRepository technicalRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _technicalRepository = technicalRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TechnicalDto> CreateAsync(CreateTechnicalDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Technical>(dto);
            await _technicalRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<TechnicalDto>(entity);
        }

        public async Task<TechnicalDto?> UpdateAsync(UpdateTechnicalDto dto, CancellationToken cancellationToken = default)
        {
            var existing = await _technicalRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _technicalRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<TechnicalDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _technicalRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _technicalRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<TechnicalDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _technicalRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<TechnicalDto>(entity);
        }

        public async Task<IEnumerable<TechnicalDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _technicalRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }

        public async Task<TechnicalDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var entity = await _technicalRepository.GetByNameAsync(name, cancellationToken);
            return entity == null ? null : _mapper.Map<TechnicalDto>(entity);
        }

        public async Task<TechnicalDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var entity = await _technicalRepository.GetByEmailAsync(email, cancellationToken);
            return entity == null ? null : _mapper.Map<TechnicalDto>(entity);
        }

        public async Task<IEnumerable<TechnicalDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var entities = await _technicalRepository.GetAllPagedAsync(page, pageSize, cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }

        public async Task<IEnumerable<TechnicalDto>> GetBySpecialtyAsync(string specialty, CancellationToken cancellationToken = default)
        {
            var entities = await _technicalRepository.GetBySpecialtyAsync(specialty, cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }

        public async Task<IEnumerable<TechnicalDto>> GetByMinimumExperienceAsync(int minExperience, CancellationToken cancellationToken = default)
        {
            var entities = await _technicalRepository.GetByMinimumExperienceAsync(minExperience, cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }

        public async Task<IEnumerable<TechnicalDto>> GetByExperienceRangeAsync(int minExperience, int maxExperience, CancellationToken cancellationToken = default)
        {
            var entities = await _technicalRepository.GetByExperienceRangeAsync(minExperience, maxExperience, cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }

        public async Task<TechnicalDto?> GetByIdWithAssessmentsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _technicalRepository.GetByIdWithAssessmentsAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<TechnicalDto>(entity);
        }

        public async Task<IEnumerable<TechnicalDto>> GetTechnicalsWithMaintenanceAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _technicalRepository.GetTechnicalsWithMaintenanceAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }

        public async Task<int> GetMaintenanceCountAsync(Guid technicalId, CancellationToken cancellationToken = default)
        {
            return await _technicalRepository.GetMaintenanceCountAsync(technicalId, cancellationToken);
        }

        public async Task<int> GetAssessmentCountAsync(Guid technicalId, CancellationToken cancellationToken = default)
        {
            return await _technicalRepository.GetAssessmentCountAsync(technicalId, cancellationToken);
        }

        public async Task<decimal?> GetAverageAssessmentScoreAsync(Guid technicalId, CancellationToken cancellationToken = default)
        {
            return await _technicalRepository.GetAverageAssessmentScoreAsync(technicalId, cancellationToken);
        }
    }
}