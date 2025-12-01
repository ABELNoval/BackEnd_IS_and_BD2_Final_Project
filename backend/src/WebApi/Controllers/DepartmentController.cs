using Application.DTOs.Department;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // ============================================================
        // GET: api/department - OBTENER TODOS
        // ============================================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _departmentService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // ============================================================
        // GET: api/department/{id} - OBTENER POR ID
        // ============================================================
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DepartmentDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _departmentService.GetByIdAsync(id, cancellationToken);
            if (result == null)
                return NotFound($"Department with id {id} not found.");

            return Ok(result);
        }

        // ============================================================
        // POST: api/department - CREAR NUEVO
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> Create(CreateDepartmentDto dto, CancellationToken cancellationToken)
        {
            var created = await _departmentService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ============================================================
        // PUT: api/department/{id} - ACTUALIZAR EXISTENTE
        // ============================================================
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<DepartmentDto>> Update(Guid id, UpdateDepartmentDto dto, CancellationToken cancellationToken)
        {
            if (id != dto.Id)
                return BadRequest("ID in URL does not match ID in body.");

            var updated = await _departmentService.UpdateAsync(dto, cancellationToken);
            if (updated == null)
                return NotFound($"Department with id {id} not found.");

            return Ok(updated);
        }

        // ============================================================
        // DELETE: api/department/{id} - ELIMINAR
        // ============================================================
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _departmentService.DeleteAsync(id, cancellationToken);
            if (!deleted)
                return NotFound($"Department with id {id} not found.");

            return NoContent();
        }
    }
}