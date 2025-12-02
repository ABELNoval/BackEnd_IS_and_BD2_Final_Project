using Application.DTOs.Responsible;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsibleController : ControllerBase
    {
        private readonly IResponsibleService _responsibleService;

        public ResponsibleController(IResponsibleService responsibleService)
        {
            _responsibleService = responsibleService;
        }

        // ============================================================
        // GET: api/responsible - OBTENER TODOS
        // ============================================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponsibleDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _responsibleService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // ============================================================
        // GET: api/responsible/{id} - OBTENER POR ID
        // ============================================================
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponsibleDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var item = await _responsibleService.GetByIdAsync(id, cancellationToken);
            if (item == null)
                return NotFound($"Responsible with id {id} not found.");

            return Ok(item);
        }

        // ============================================================
        // POST: api/responsible - CREAR NUEVO
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<ResponsibleDto>> Create(CreateResponsibleDto dto, CancellationToken cancellationToken)
        {
            var created = await _responsibleService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ============================================================
        // PUT: api/responsible/{id} - ACTUALIZAR EXISTENTE
        // ============================================================
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponsibleDto>> Update(Guid id, UpdateResponsibleDto dto, CancellationToken cancellationToken)
        {
            if (id != dto.Id)
                return BadRequest("ID in URL does not match ID in body.");

            var updated = await _responsibleService.UpdateAsync(dto, cancellationToken);
            if (updated == null)
                return NotFound($"Responsible with id {id} not found.");

            return Ok(updated);
        }

        // ============================================================
        // DELETE: api/responsible/{id} - ELIMINAR
        // ============================================================
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _responsibleService.DeleteAsync(id, cancellationToken);
            if (!deleted)
                return NotFound($"Responsible with id {id} not found.");

            return NoContent();
        }

        // ============================================================
        // POST: api/responsible/filter
        // ============================================================
        [HttpPost("filter")]
        public async Task<ActionResult> Filter([FromBody] List<string> request, CancellationToken cancellationToken)
        {
            string query = "";

            if (request != null && request.Count > 0)
            {
                query = string.Join(" AND ", request);
            }

            var result = await _responsibleService.FilterAsync(query, cancellationToken);

            return Ok(result);
        }
    }
}