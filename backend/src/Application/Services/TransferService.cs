using Application.DTOs.Transfer;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransferService(
            ITransferRepository transferRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _transferRepository = transferRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Crear una transferencia
        public async Task<TransferDto> CreateAsync(CreateTransferDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Transfer>(dto);

            await _transferRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransferDto>(entity);
        }

        // Actualizar una transferencia
        public async Task<TransferDto?> UpdateAsync(UpdateTransferDto dto, CancellationToken cancellationToken = default)
        {
            var existing = await _transferRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _transferRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransferDto>(existing);
        }

        // Eliminar una transferencia
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _transferRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _transferRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        // Obtener una transferencia por Id
        public async Task<TransferDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _transferRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<TransferDto>(entity);
        }

        // Obtener todas las transferencias
        public async Task<IEnumerable<TransferDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _transferRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener transferencias por equipo
        public async Task<IEnumerable<TransferDto>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            var entities = await _transferRepository.GetByEquipmentIdAsync(equipmentId, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener transferencias por departamento de origen
        public async Task<IEnumerable<TransferDto>> GetBySourceDepartmentIdAsync(Guid sourceDepartmentId, CancellationToken cancellationToken = default)
        {
            var entities = await _transferRepository.GetBySourceDepartmentIdAsync(sourceDepartmentId, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener transferencias por departamento destino
        public async Task<IEnumerable<TransferDto>> GetByTargetDepartmentIdAsync(Guid targetDepartmentId, CancellationToken cancellationToken = default)
        {
            var entities = await _transferRepository.GetByTargetDepartmentIdAsync(targetDepartmentId, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener transferencias por departamento (origen o destino)
        public async Task<IEnumerable<TransferDto>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            var entities = await _transferRepository.GetByDepartmentIdAsync(departmentId, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener transferencias por responsable
        public async Task<IEnumerable<TransferDto>> GetByResponsibleIdAsync(Guid responsibleId, CancellationToken cancellationToken = default)
        {
            var entities = await _transferRepository.GetByResponsibleIdAsync(responsibleId, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener transferencias por rango de fechas
        public async Task<IEnumerable<TransferDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var entities = await _transferRepository.GetByDateRangeAsync(startDate, endDate, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener transferencias recientes
        public async Task<IEnumerable<TransferDto>> GetRecentTransfersAsync(int count, CancellationToken cancellationToken = default)
        {
            var entities = await _transferRepository.GetRecentTransfersAsync(count, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener la última transferencia de un equipo
        public async Task<TransferDto?> GetLatestByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            var entity = await _transferRepository.GetLatestByEquipmentIdAsync(equipmentId, cancellationToken);
            return entity == null ? null : _mapper.Map<TransferDto>(entity);
        }

        // Contar transferencias por equipo
        public async Task<int> CountByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            return await _transferRepository.CountByEquipmentIdAsync(equipmentId, cancellationToken);
        }

        // Contar transferencias por departamento
        public async Task<int> CountByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            return await _transferRepository.CountByDepartmentIdAsync(departmentId, cancellationToken);
        }
    }
}
