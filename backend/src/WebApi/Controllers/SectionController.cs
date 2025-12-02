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
        // GET: api/section - OBTENER TODOS
        // ================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SectionDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _sectionService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // ================================
        // GET: api/section/{id} - OBTENER POR ID
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
        // POST: api/section - CREAR NUEVO
        // ================================
        [HttpPost]
        public async Task<ActionResult<SectionDto>> Create(CreateSectionDto dto, CancellationToken cancellationToken)
        {
            var created = await _sectionService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ================================
        // PUT: api/section/{id} - ACTUALIZAR EXISTENTE
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
        // DELETE: api/section/{id} - ELIMINAR
        // ================================
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _sectionService.DeleteAsync(id, cancellationToken);
            if (!deleted)
                return NotFound($"Section with id {id} not found.");

            return NoContent();
        }
    }
}