using Application.DTOs.Equipment;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EquipmentService(
            IEquipmentRepository equipmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _equipmentRepository = equipmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EquipmentDto> CreateAsync(CreateEquipmentDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Equipment>(dto);
            await _equipmentRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EquipmentDto>(entity);
        }

        public async Task<EquipmentDto?> UpdateAsync(UpdateEquipmentDto dto, CancellationToken cancellationToken = default)
        {
            var existing = await _equipmentRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _equipmentRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EquipmentDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _equipmentRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _equipmentRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<EquipmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _equipmentRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<EquipmentDto>(entity);
        }

        public async Task<EquipmentDto?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _equipmentRepository.GetByIdWithDetailsAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<EquipmentDto>(entity);
        }

        public async Task<IEnumerable<EquipmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<EquipmentDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var entity = await _equipmentRepository.GetByNameAsync(name, cancellationToken);
            return entity == null ? null : _mapper.Map<EquipmentDto>(entity);
        }

        public async Task<IEnumerable<EquipmentDto>> GetByEquipmentTypeIdAsync(Guid equipmentTypeId, CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetByEquipmentTypeIdAsync(equipmentTypeId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetByDepartmentIdAsync(departmentId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetByStateAsync(int stateId, CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetByStateAsync(stateId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetByLocationTypeAsync(int locationTypeId, CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetByLocationTypeAsync(locationTypeId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetByAcquisitionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetByAcquisitionDateRangeAsync(startDate, endDate, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetAllPagedAsync(page, pageSize, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetOperativeByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetOperativeByDepartmentIdAsync(departmentId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetEquipmentUnderMaintenanceAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetEquipmentUnderMaintenanceAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetWarehouseEquipmentAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetWarehouseEquipmentAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _equipmentRepository.ExistsByNameAsync(name, cancellationToken);
        }

        public async Task<int> CountByStateAsync(int stateId, CancellationToken cancellationToken = default)
        {
            return await _equipmentRepository.CountByStateAsync(stateId, cancellationToken);
        }
    }
}