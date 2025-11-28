using Application.DTOs.EquipmentType;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class EquipmentTypeService : IEquipmentTypeService
    {
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EquipmentTypeService(
            IEquipmentTypeRepository equipmentTypeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _equipmentTypeRepository = equipmentTypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EquipmentTypeDto> CreateAsync(CreateEquipmentTypeDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<EquipmentType>(dto);
            await _equipmentTypeRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EquipmentTypeDto>(entity);
        }

        public async Task<EquipmentTypeDto?> UpdateAsync(UpdateEquipmentTypeDto dto, CancellationToken cancellationToken = default)
        {
            var existing = await _equipmentTypeRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _equipmentTypeRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EquipmentTypeDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _equipmentTypeRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _equipmentTypeRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<EquipmentTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _equipmentTypeRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<EquipmentTypeDto>(entity);
        }

        public async Task<IEnumerable<EquipmentTypeDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentTypeRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentTypeDto>>(entities);
        }

        public async Task<EquipmentTypeDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var entity = await _equipmentTypeRepository.GetByNameAsync(name, cancellationToken);
            return entity == null ? null : _mapper.Map<EquipmentTypeDto>(entity);
        }

        public async Task<IEnumerable<EquipmentTypeDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentTypeRepository.GetAllPagedAsync(page, pageSize, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentTypeDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentTypeDto>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentTypeRepository.SearchByNameAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentTypeDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentTypeDto>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentTypeRepository.GetAllOrderedByNameAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentTypeDto>>(entities);
        }

        public async Task<int> GetEquipmentCountByTypeIdAsync(Guid equipmentTypeId, CancellationToken cancellationToken = default)
        {
            return await _equipmentTypeRepository.GetEquipmentCountByTypeIdAsync(equipmentTypeId, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _equipmentTypeRepository.ExistsByNameAsync(name, cancellationToken);
        }
    }
}