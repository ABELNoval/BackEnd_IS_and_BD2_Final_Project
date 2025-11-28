using Application.DTOs.Section;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SectionService(
            ISectionRepository sectionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _sectionRepository = sectionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SectionDto> CreateAsync(CreateSectionDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Section>(dto);
            await _sectionRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<SectionDto>(entity);
        }

        public async Task<SectionDto?> UpdateAsync(UpdateSectionDto dto, CancellationToken cancellationToken = default)
        {
            var existing = await _sectionRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _sectionRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<SectionDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _sectionRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _sectionRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<SectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _sectionRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<SectionDto>(entity);
        }

        public async Task<IEnumerable<SectionDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _sectionRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<SectionDto>>(entities);
        }

        public async Task<SectionDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var entity = await _sectionRepository.GetByNameAsync(name, cancellationToken);
            return entity == null ? null : _mapper.Map<SectionDto>(entity);
        }

        public async Task<IEnumerable<SectionDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var entities = await _sectionRepository.GetAllPagedAsync(page, pageSize, cancellationToken);
            return _mapper.Map<IEnumerable<SectionDto>>(entities);
        }

        public async Task<IEnumerable<SectionDto>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            var entities = await _sectionRepository.SearchByNameAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<SectionDto>>(entities);
        }

        public async Task<IEnumerable<SectionDto>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _sectionRepository.GetAllOrderedByNameAsync(cancellationToken);
            return _mapper.Map<IEnumerable<SectionDto>>(entities);
        }

        public async Task<int> GetDepartmentCountBySectionIdAsync(Guid sectionId, CancellationToken cancellationToken = default)
        {
            return await _sectionRepository.GetDepartmentCountBySectionIdAsync(sectionId, cancellationToken);
        }

        public async Task<bool> HasDepartmentsAsync(Guid sectionId, CancellationToken cancellationToken = default)
        {
            return await _sectionRepository.HasDepartmentsAsync(sectionId, cancellationToken);
        }
    }
}