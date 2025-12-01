using Application.DTOs.Section;
using Application.Interfaces.Services;
using Application.Validators.Section;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateSectionDto> _createValidator;
        private readonly IValidator<UpdateSectionDto> _updateValidator;

        public SectionService(
            ISectionRepository sectionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateSectionDto> createValidator,
            IValidator<UpdateSectionDto> updateValidator)
        {
            _sectionRepository = sectionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<SectionDto> CreateAsync(CreateSectionDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Section>(dto);
            await _sectionRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<SectionDto>(entity);
        }

        public async Task<SectionDto?> UpdateAsync(UpdateSectionDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var existing = await _sectionRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _sectionRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<SectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

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
            // Validación básica del nombre
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Section name cannot be empty", nameof(name));
            }

            var entity = await _sectionRepository.GetByNameAsync(name, cancellationToken);
            return entity == null ? null : _mapper.Map<SectionDto>(entity);
        }

        public async Task<IEnumerable<SectionDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            // Validación de paginación
            if (page < 1)
            {
                throw new ArgumentException("Page number must be greater than 0", nameof(page));
            }

            if (pageSize < 1 || pageSize > 100)
            {
                throw new ArgumentException("Page size must be between 1 and 100", nameof(pageSize));
            }

            var entities = await _sectionRepository.GetAllPagedAsync(page, pageSize, cancellationToken);
            return _mapper.Map<IEnumerable<SectionDto>>(entities);
        }

        public async Task<IEnumerable<SectionDto>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            // Validación del término de búsqueda
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException("Search term cannot be empty", nameof(searchTerm));
            }

            // Validación de longitud mínima para búsqueda
            if (searchTerm.Length < 2)
            {
                throw new ArgumentException("Search term must be at least 2 characters", nameof(searchTerm));
            }

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
            // Validación básica del ID
            if (sectionId == Guid.Empty)
            {
                throw new ArgumentException("Section ID cannot be empty", nameof(sectionId));
            }

            return await _sectionRepository.GetDepartmentCountBySectionIdAsync(sectionId, cancellationToken);
        }

        public async Task<bool> HasDepartmentsAsync(Guid sectionId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (sectionId == Guid.Empty)
            {
                throw new ArgumentException("Section ID cannot be empty", nameof(sectionId));
            }

            return await _sectionRepository.HasDepartmentsAsync(sectionId, cancellationToken);
        }
    }
}