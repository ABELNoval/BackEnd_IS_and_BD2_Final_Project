using Application.DTOs.Employee;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // =========================================
        // GET: api/employee
        // =========================================
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _employeeService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // =========================================
        // GET: api/employee/{id}
        // =========================================
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _employeeService.GetByIdAsync(id, cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        // =========================================
        // POST: api/employee
        // =========================================
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto, CancellationToken cancellationToken)
        {
            var created = await _employeeService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // =========================================
        // PUT: api/employee/{id}
        // =========================================
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateEmployeeDto dto, CancellationToken cancellationToken)
        {
            if (dto.Id != id)
                dto.Id = id;

            var updated = await _employeeService.UpdateAsync(dto, cancellationToken);
            return updated == null ? NotFound() : Ok(updated);
        }

        // =========================================
        // DELETE: api/employee/{id}
        // =========================================
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var success = await _employeeService.DeleteAsync(id, cancellationToken);
            return success ? NoContent() : NotFound();
        }

        // ===== Extra endpoints =====

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
        {
            var result = await _employeeService.GetByNameAsync(name, cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetByEmail(string email, CancellationToken cancellationToken)
        {
            var result = await _employeeService.GetByEmailAsync(email, cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("exists/{email}")]
        public async Task<IActionResult> ExistsByEmail(string email, CancellationToken cancellationToken)
        {
            var exists = await _employeeService.ExistsByEmailAsync(email, cancellationToken);
            return Ok(new { exists });
        }

        [HttpGet("paged/{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetPaged(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var result = await _employeeService.GetAllPagedAsync(pageNumber, pageSize, cancellationToken);
            return Ok(result);
        }
    }
}
