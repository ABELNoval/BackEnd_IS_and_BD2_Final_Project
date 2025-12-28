using Application.DTOs.Department;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == "Responsible")
            {
                var sectionIdClaim = User.FindFirst("SectionId")?.Value;
                if (Guid.TryParse(sectionIdClaim, out var sectionId))
                {
                    var result = await _departmentService.GetBySectionIdAsync(sectionId, cancellationToken);
                    return Ok(result);
                }
            }
            else if (role == "Employee")
            {
                var departmentIdClaim = User.FindFirst("DepartmentId")?.Value;
                if (Guid.TryParse(departmentIdClaim, out var departmentId))
                {
                    var department = await _departmentService.GetByIdAsync(departmentId, cancellationToken);
                    return Ok(department != null ? new List<DepartmentDto> { department } : new List<DepartmentDto>());
                }
            }

            var allResult = await _departmentService.GetAllAsync(cancellationToken);
            return Ok(allResult);
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

        // =========================================
        // GET: api/department/filter
        // =========================================
        [HttpPost("filter")]
        public async Task<IActionResult> Filter([FromBody] List<string> request, CancellationToken cancellationToken)
        {
            string query = "";

            if (request != null && request.Count > 0)
            {
                query = string.Join(" AND ", request);
            }

            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == "Responsible")
            {
                var sectionIdClaim = User.FindFirst("SectionId")?.Value;
                if (Guid.TryParse(sectionIdClaim, out var sectionId))
                {
                    var sectionFilter = $"SectionId == \"{sectionId}\"";
                    if (!string.IsNullOrEmpty(query))
                    {
                        query = $"({query}) AND {sectionFilter}";
                    }
                    else
                    {
                        query = sectionFilter;
                    }
                }
            }

            var result = await _departmentService.FilterAsync(query, cancellationToken);

            return Ok(result);
        }

    }
}