using Application.DTOs.Responsible;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class ResponsibleService : IResponsibleService
    {
        private readonly IResponsibleRepository _responsibleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ResponsibleService(
            IResponsibleRepository responsibleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _responsibleRepository = responsibleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponsibleDto> CreateAsync(CreateResponsibleDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Responsible>(dto);
            await _responsibleRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ResponsibleDto>(entity);
        }

        public async Task<ResponsibleDto?> UpdateAsync(UpdateResponsibleDto dto, CancellationToken cancellationToken = default)
        {
            var existing = await _responsibleRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _responsibleRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ResponsibleDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _responsibleRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _responsibleRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<ResponsibleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _responsibleRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<ResponsibleDto>(entity);
        }

        public async Task<IEnumerable<ResponsibleDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _responsibleRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ResponsibleDto>>(entities);
        }

        public async Task<ResponsibleDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var entity = await _responsibleRepository.GetByNameAsync(name, cancellationToken);
            return entity == null ? null : _mapper.Map<ResponsibleDto>(entity);
        }

        public async Task<ResponsibleDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var entity = await _responsibleRepository.GetByEmailAsync(email, cancellationToken);
            return entity == null ? null : _mapper.Map<ResponsibleDto>(entity);
        }

        public async Task<IEnumerable<ResponsibleDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var entities = await _responsibleRepository.GetAllPagedAsync(page, pageSize, cancellationToken);
            return _mapper.Map<IEnumerable<ResponsibleDto>>(entities);
        }

        public async Task<ResponsibleDto?> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            var entity = await _responsibleRepository.GetByDepartmentIdAsync(departmentId, cancellationToken);
            return entity == null ? null : _mapper.Map<ResponsibleDto>(entity);
        }

        public async Task<IEnumerable<ResponsibleDto>> GetResponsiblesWithTransfersAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _responsibleRepository.GetResponsiblesWithTransfersAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ResponsibleDto>>(entities);
        }

        public async Task<int> GetTransferCountAsync(Guid responsibleId, CancellationToken cancellationToken = default)
        {
            return await _responsibleRepository.GetTransferCountAsync(responsibleId, cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _responsibleRepository.ExistsByEmailAsync(email, cancellationToken);
        }

        public async Task<bool> IsManagingDepartmentAsync(Guid responsibleId, CancellationToken cancellationToken = default)
        {
            return await _responsibleRepository.IsManagingDepartmentAsync(responsibleId, cancellationToken);
        }
    }
}