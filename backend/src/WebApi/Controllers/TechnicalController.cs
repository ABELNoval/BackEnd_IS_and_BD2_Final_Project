using Application.DTOs.Technical;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TechnicalController : ControllerBase
    {
        private readonly ITechnicalService _technicalService;

        public TechnicalController(ITechnicalService technicalService)
        {
            _technicalService = technicalService;
        }

        // ================================
        // GET: api/technical
        // ================================
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _technicalService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // ================================
        // GET: api/technical/{id}
        // ================================
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _technicalService.GetByIdAsync(id, cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        // ================================
        // POST: api/technical
        // ================================
        [HttpPost]
        public async Task<IActionResult> Create(CreateTechnicalDto dto, CancellationToken cancellationToken)
        {
            var created = await _technicalService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ================================
        // PUT: api/technical/{id}
        // ================================
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateTechnicalDto dto, CancellationToken cancellationToken)
        {
            if (dto.Id != id)
                dto.Id = id;

            var updated = await _technicalService.UpdateAsync(dto, cancellationToken);
            return updated == null ? NotFound() : Ok(updated);
        }

        // ================================
        // DELETE: api/technical/{id}
        // ================================
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _technicalService.DeleteAsync(id, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
    }
}
