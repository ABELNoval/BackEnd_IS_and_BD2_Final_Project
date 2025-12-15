using Application.DTOs.Section;
using Application.Interfaces.Services;
using Application.Validators.Section;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Application.Exceptions;
using Domain.ValueObjects;

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

        /// <summary>
        /// Creates a new section entity after validating the DTO.
        /// </summary>
        /// <param name="dto">The CreateSectionDto to validate and create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created SectionDto.</returns>
        public async Task<SectionDto> CreateAsync(CreateSectionDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var entity = Section.Create(dto.Name);
            await _sectionRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<SectionDto>(entity);
        }


        /// <summary>
        /// Updates an existing section entity after validating the DTO.
        /// </summary>
        /// <param name="dto">The UpdateSectionDto to validate and update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated SectionDto, or throws EntityNotFoundException if not found.</returns>
        public async Task<SectionDto?> UpdateAsync(UpdateSectionDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var existing = await _sectionRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Section), dto.Id);

            existing.UpdateBasicInfo(dto.Name);
            await _sectionRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<SectionDto>(existing);
        }


        /// <summary>
        /// Deletes a section entity by ID.
        /// </summary>
        /// <param name="id">The section ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deleted, otherwise throws EntityNotFoundException.</returns>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var existing = await _sectionRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Section), id);

            await _sectionRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Gets a section entity by ID.
        /// </summary>
        /// <param name="id">The section ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The SectionDto if found, otherwise throws EntityNotFoundException.</returns>
        public async Task<SectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var entity = await _sectionRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new EntityNotFoundException(nameof(Section), id);
            return _mapper.Map<SectionDto>(entity);
        }

        public async Task<IEnumerable<SectionDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _sectionRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<SectionDto>>(entities);
        }

        public async Task<IEnumerable<SectionDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _sectionRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<SectionDto>>(entities);
        }
    }
}