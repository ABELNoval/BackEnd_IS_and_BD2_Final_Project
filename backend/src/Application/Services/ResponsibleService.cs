using Application.DTOs.Responsible;
using Application.Interfaces.Services;
using Application.Validators.Responsible;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Application.Exceptions;
using Domain.ValueObjects;

namespace Application.Services
{
    public class ResponsibleService : IResponsibleService
    {
        private readonly IResponsibleRepository _responsibleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateResponsibleDto> _createValidator;
        private readonly IValidator<UpdateResponsibleDto> _updateValidator;

        public ResponsibleService(
            IResponsibleRepository responsibleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateResponsibleDto> createValidator,
            IValidator<UpdateResponsibleDto> updateValidator)
        {
            _responsibleRepository = responsibleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Creates a new responsible after validating the DTO.
        /// </summary>
        /// <param name="dto">The CreateResponsibleDto to validate and create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created ResponsibleDto.</returns>
        public async Task<ResponsibleDto> CreateAsync(CreateResponsibleDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var email = Email.Create(dto.Email);               
            var password = PasswordHash.Create(dto.Password);   

            var entity = Responsible.Create(dto.Name, email, password, dto.DepartmentId);
            await _responsibleRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ResponsibleDto>(entity);
        }

        /// <summary>
        /// Updates an existing responsible after validating the DTO.
        /// </summary>
        /// <param name="dto">The UpdateResponsibleDto to validate and update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated ResponsibleDto, or throws EntityNotFoundException if not found.</returns>
        public async Task<ResponsibleDto?> UpdateAsync(UpdateResponsibleDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var existing = await _responsibleRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Responsible), dto.Id);

            var updatedEmail = existing.Email.Update(dto.Email);
            var updatedPasswordHash = string.IsNullOrWhiteSpace(dto.Password) 
                ? existing.PasswordHash   
                : existing.PasswordHash.Update(dto.Password);

            existing.Update(
                dto.Name,
                updatedEmail,               
                updatedPasswordHash      
            );

            await _responsibleRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ResponsibleDto>(existing);
        }


        /// <summary>
        /// Deletes a responsible by ID.
        /// </summary>
        /// <param name="id">The responsible ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deleted, otherwise throws EntityNotFoundException.</returns>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var existing = await _responsibleRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Responsible), id);

            await _responsibleRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Gets a responsible by ID.
        /// </summary>
        /// <param name="id">The responsible ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The ResponsibleDto if found, otherwise throws EntityNotFoundException.</returns>
        public async Task<ResponsibleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var entity = await _responsibleRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new EntityNotFoundException(nameof(Responsible), id);
            return _mapper.Map<ResponsibleDto>(entity);
        }

        public async Task<IEnumerable<ResponsibleDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _responsibleRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ResponsibleDto>>(entities);
        }

        public async Task<IEnumerable<ResponsibleDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _responsibleRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<ResponsibleDto>>(entities);
        }
    }
}