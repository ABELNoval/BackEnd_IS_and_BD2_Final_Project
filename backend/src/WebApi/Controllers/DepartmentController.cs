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
        // GET: api/department
        // ============================================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _departmentService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // ============================================================
        // GET: api/department/{id}
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
        // POST: api/department
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> Create(CreateDepartmentDto dto, CancellationToken cancellationToken)
        {
            var created = await _departmentService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ============================================================
        // PUT: api/department/{id}
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
        // DELETE: api/department/{id}
        // ============================================================
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _departmentService.DeleteAsync(id, cancellationToken);
            if (!deleted)
                return NotFound($"Department with id {id} not found.");

            return NoContent();
        }

        // ============================================================
        // 🔍 EXTRA ENDPOINTS ÚTILES
        // ============================================================

        // GET: api/department/byName/{name}
        [HttpGet("byName/{name}")]
        public async Task<ActionResult<DepartmentDto>> GetByName(string name, CancellationToken cancellationToken)
        {
            var result = await _departmentService.GetByNameAsync(name, cancellationToken);
            if (result == null)
                return NotFound($"No department found with name '{name}'.");

            return Ok(result);
        }

        // GET: api/department/section/{sectionId}
        [HttpGet("section/{sectionId:guid}")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetBySection(Guid sectionId, CancellationToken cancellationToken)
        {
            return Ok(await _departmentService.GetBySectionIdAsync(sectionId, cancellationToken));
        }

        // GET: api/department/responsible/{personId}
        [HttpGet("responsible/{responsibleId:guid}")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetByResponsible(Guid responsibleId, CancellationToken cancellationToken)
        {
            return Ok(await _departmentService.GetByResponsibleIdAsync(responsibleId, cancellationToken));
        }

        // ============================================================
        // PAGINACIÓN
        // GET: api/department/paged?page=1&pageSize=10
        // ============================================================
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return Ok(await _departmentService.GetAllPagedAsync(page, pageSize, cancellationToken));
        }
    }
}
