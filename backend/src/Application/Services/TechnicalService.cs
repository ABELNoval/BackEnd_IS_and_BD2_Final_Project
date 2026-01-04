using Application.DTOs.Technical;
using Application.Interfaces.Services;
using Application.Validators.Technical;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Application.Exceptions;
using Domain.ValueObjects;


namespace Application.Services
{
    public class TechnicalService : ITechnicalService
    {
        private readonly ITechnicalRepository _technicalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTechnicalDto> _createValidator;
        private readonly IValidator<UpdateTechnicalDto> _updateValidator;

        public TechnicalService(
            ITechnicalRepository technicalRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateTechnicalDto> createValidator,
            IValidator<UpdateTechnicalDto> updateValidator)
        {
            _technicalRepository = technicalRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Creates a new technical after validating the DTO.
        /// </summary>
        /// <param name="dto">The CreateTechnicalDto to validate and create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created TechnicalDto.</returns>
        public async Task<TechnicalDto> CreateAsync(CreateTechnicalDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var entity = Technical.Create(
                dto.Name,
                Email.Create(dto.Email),
                PasswordHash.Create(dto.Password),
                dto.Experience,
                dto.Specialty
            );

            await _technicalRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TechnicalDto>(entity);
        }


        /// <summary>
        /// Updates an existing technical after validating the DTO.
        /// </summary>
        /// <param name="dto">The UpdateTechnicalDto to validate and update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated TechnicalDto, or throws EntityNotFoundException if not found.</returns>
        public async Task<TechnicalDto?> UpdateAsync(UpdateTechnicalDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var existing = await _technicalRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Technical), dto.Id);

            var updatedEmail = existing.Email.Update(dto.Email);
            var updatedPasswordHash = string.IsNullOrWhiteSpace(dto.Password) 
                ? existing.PasswordHash   
                : existing.PasswordHash.Update(dto.Password);

            existing.UpdateBasicInfo(
                dto.Name,
                updatedEmail,
                updatedPasswordHash,
                dto.Experience,
                dto.Specialty
            );

            await _technicalRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TechnicalDto>(existing);
        }


        /// <summary>
        /// Deletes a technical by ID.
        /// </summary>
        /// <param name="id">The technical ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deleted, otherwise throws EntityNotFoundException.</returns>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var existing = await _technicalRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Technical), id);

            await _technicalRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Gets a technical by ID.
        /// </summary>
        /// <param name="id">The technical ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The TechnicalDto if found, otherwise throws EntityNotFoundException.</returns>
        public async Task<TechnicalDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var entity = await _technicalRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new EntityNotFoundException(nameof(Technical), id);
            return _mapper.Map<TechnicalDto>(entity);
        }

        public async Task<IEnumerable<TechnicalDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _technicalRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }

        public async Task<IEnumerable<TechnicalDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _technicalRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }
    }
}