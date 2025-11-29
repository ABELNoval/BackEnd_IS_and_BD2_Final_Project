using Application.DTOs.Section;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionController : ControllerBase
    {
        private readonly ISectionService _sectionService;

        public SectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        // ================================
        // GET: api/section
        // ================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SectionDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _sectionService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // ================================
        // GET: api/section/{id}
        // ================================
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SectionDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var item = await _sectionService.GetByIdAsync(id, cancellationToken);
            if (item == null)
                return NotFound($"Section with id {id} not found.");

            return Ok(item);
        }

        // ================================
        // POST: api/section
        // ================================
        [HttpPost]
        public async Task<ActionResult<SectionDto>> Create(CreateSectionDto dto, CancellationToken cancellationToken)
        {
            var created = await _sectionService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ================================
        // PUT: api/section/{id}
        // ================================
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SectionDto>> Update(Guid id, UpdateSectionDto dto, CancellationToken cancellationToken)
        {
            if (id != dto.Id)
                return BadRequest("ID in URL does not match ID in body.");

            var updated = await _sectionService.UpdateAsync(dto, cancellationToken);
            if (updated == null)
                return NotFound($"Section with id {id} not found.");

            return Ok(updated);
        }

        // ================================
        // DELETE: api/section/{id}
        // ================================
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _sectionService.DeleteAsync(id, cancellationToken);
            if (!deleted)
                return NotFound($"Section with id {id} not found.");

            return NoContent();
        }

        // ===========================================================
        // 🔍 EXTRA ENDPOINTS
        // ===========================================================

        [HttpGet("byName/{name}")]
        public async Task<ActionResult<SectionDto>> GetByName(string name, CancellationToken cancellationToken)
        {
            var result = await _sectionService.GetByNameAsync(name, cancellationToken);
            if (result == null)
                return NotFound($"No section found with name '{name}'.");

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<SectionDto>>> Search([FromQuery] string term, CancellationToken cancellationToken)
        {
            var result = await _sectionService.SearchByNameAsync(term, cancellationToken);
            return Ok(result);
        }

        [HttpGet("ordered")]
        public async Task<ActionResult<IEnumerable<SectionDto>>> GetOrdered(CancellationToken cancellationToken)
        {
            var result = await _sectionService.GetAllOrderedByNameAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}/departmentCount")]
        public async Task<ActionResult<int>> GetDepartmentCount(Guid id, CancellationToken cancellationToken)
        {
            var count = await _sectionService.GetDepartmentCountBySectionIdAsync(id, cancellationToken);
            return Ok(count);
        }

        [HttpGet("{id:guid}/hasDepartments")]
        public async Task<ActionResult<bool>> HasDepartments(Guid id, CancellationToken cancellationToken)
        {
            var has = await _sectionService.HasDepartmentsAsync(id, cancellationToken);
            return Ok(has);
        }

        // ================================
        // PAGINACIÓN
        // GET: api/section/paged?page=1&pageSize=10
        // ================================
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<SectionDto>>> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _sectionService.GetAllPagedAsync(page, pageSize, cancellationToken);
            return Ok(result);
        }
    }
}
