using Application.DTOs.Director;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorController : ControllerBase
    {
        private readonly IDirectorService _directorService;

        public DirectorController(IDirectorService directorService)
        {
            _directorService = directorService;
        }

        // =========================================
        // GET: api/director
        // =========================================
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _directorService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // =========================================
        // GET: api/director/{id}
        // =========================================
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _directorService.GetByIdAsync(id, cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        // =========================================
        // POST: api/director
        // =========================================
        [HttpPost]
        public async Task<IActionResult> Create(CreateDirectorDto dto, CancellationToken cancellationToken)
        {
            var created = await _directorService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // =========================================
        // PUT: api/director/{id}
        // =========================================
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateDirectorDto dto, CancellationToken cancellationToken)
        {
            if (dto.Id != id)
                dto.Id = id;

            var updated = await _directorService.UpdateAsync(dto, cancellationToken);
            return updated == null ? NotFound() : Ok(updated);
        }

        // =========================================
        // DELETE: api/director/{id}
        // =========================================
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var success = await _directorService.DeleteAsync(id, cancellationToken);
            return success ? NoContent() : NotFound();
        }

        // ===== Extra endpoints =====

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
        {
            var result = await _directorService.GetByNameAsync(name, cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetByEmail(string email, CancellationToken cancellationToken)
        {
            var result = await _directorService.GetByEmailAsync(email, cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("exists/{email}")]
        public async Task<IActionResult> ExistsByEmail(string email, CancellationToken cancellationToken)
        {
            var exists = await _directorService.ExistsByEmailAsync(email, cancellationToken);
            return Ok(new { exists });
        }

        [HttpGet("paged/{page:int}/{size:int}")]
        public async Task<IActionResult> GetPaged(int page, int size, CancellationToken cancellationToken)
        {
            var result = await _directorService.GetAllPagedAsync(page, size, cancellationToken);
            return Ok(result);
        }
    }
}
