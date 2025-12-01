using Application.DTOs.Employee;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Employee>(dto);
            await _employeeRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EmployeeDto>(entity);
        }

        public async Task<EmployeeDto?> UpdateAsync(UpdateEmployeeDto dto, CancellationToken cancellationToken = default)
        {
            var existing = await _employeeRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _employeeRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EmployeeDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _employeeRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<EmployeeDto>(entity);
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _employeeRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EmployeeDto>>(entities);
        }

        public async Task<EmployeeDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var entity = await _employeeRepository.GetByNameAsync(name, cancellationToken);
            return entity == null ? null : _mapper.Map<EmployeeDto>(entity);
        }

        public async Task<EmployeeDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var entity = await _employeeRepository.GetByEmailAsync(email, cancellationToken);
            return entity == null ? null : _mapper.Map<EmployeeDto>(entity);
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var entities = await _employeeRepository.GetAllPagedAsync(pageNumber, pageSize, cancellationToken);
            return _mapper.Map<IEnumerable<EmployeeDto>>(entities);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _employeeRepository.ExistsByEmailAsync(email, cancellationToken);
        }
    }
}