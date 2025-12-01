using Application.DTOs.Department;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Department>(dto);
            await _departmentRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<DepartmentDto>(entity);
        }

        public async Task<DepartmentDto?> UpdateAsync(UpdateDepartmentDto dto, CancellationToken cancellationToken = default)
        {
            var existing = await _departmentRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _departmentRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<DepartmentDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _departmentRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _departmentRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<DepartmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _departmentRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<DepartmentDto>(entity);
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _departmentRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<DepartmentDto>>(entities);
        }

        public async Task<IEnumerable<DepartmentDto>> GetBySectionIdAsync(Guid sectionId, CancellationToken cancellationToken = default)
        {
            var entities = await _departmentRepository.GetBySectionIdAsync(sectionId);
            return _mapper.Map<IEnumerable<DepartmentDto>>(entities);
        }

        public async Task<IEnumerable<DepartmentDto>> GetByResponsibleIdAsync(Guid responsibleId, CancellationToken cancellationToken = default)
        {
            var entities = await _departmentRepository.GetByResponsibleIdAsync(responsibleId);
            return _mapper.Map<IEnumerable<DepartmentDto>>(entities);
        }

        public async Task<DepartmentDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var entity = await _departmentRepository.GetByNameAsync(name);
            return entity == null ? null : _mapper.Map<DepartmentDto>(entity);
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var entities = await _departmentRepository.GetAllPagedAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<DepartmentDto>>(entities);
        }
    }
}