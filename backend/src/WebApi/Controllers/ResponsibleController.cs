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
        // GET: api/responsible
        // ============================================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponsibleDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _responsibleService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // ============================================================
        // GET: api/responsible/{id}
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
        // POST: api/responsible
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<ResponsibleDto>> Create(CreateResponsibleDto dto, CancellationToken cancellationToken)
        {
            var created = await _responsibleService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ============================================================
        // PUT: api/responsible/{id}
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
        // DELETE: api/responsible/{id}
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
        // EXTRA ENDPOINTS
        // ============================================================

        [HttpGet("byName/{name}")]
        public async Task<ActionResult<ResponsibleDto>> GetByName(string name, CancellationToken cancellationToken)
        {
            var result = await _responsibleService.GetByNameAsync(name, cancellationToken);
            if (result == null)
                return NotFound($"No responsible found with name '{name}'.");

            return Ok(result);
        }

        [HttpGet("byEmail/{email}")]
        public async Task<ActionResult<ResponsibleDto>> GetByEmail(string email, CancellationToken cancellationToken)
        {
            var result = await _responsibleService.GetByEmailAsync(email, cancellationToken);
            if (result == null)
                return NotFound($"No responsible found with email '{email}'.");

            return Ok(result);
        }

        [HttpGet("department/{departmentId:guid}")]
        public async Task<ActionResult<ResponsibleDto>> GetByDepartment(Guid departmentId, CancellationToken cancellationToken)
        {
            var result = await _responsibleService.GetByDepartmentIdAsync(departmentId, cancellationToken);
            if (result == null)
                return NotFound($"No responsible found for department '{departmentId}'.");

            return Ok(result);
        }

        [HttpGet("withTransfers")]
        public async Task<ActionResult<IEnumerable<ResponsibleDto>>> GetResponsiblesWithTransfers(CancellationToken cancellationToken)
        {
            var result = await _responsibleService.GetResponsiblesWithTransfersAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}/transferCount")]
        public async Task<ActionResult<int>> GetTransferCount(Guid id, CancellationToken cancellationToken)
        {
            var count = await _responsibleService.GetTransferCountAsync(id, cancellationToken);
            return Ok(count);
        }

        [HttpGet("{id:guid}/isManagingDepartment")]
        public async Task<ActionResult<bool>> IsManagingDepartment(Guid id, CancellationToken cancellationToken)
        {
            var isManaging = await _responsibleService.IsManagingDepartmentAsync(id, cancellationToken);
            return Ok(isManaging);
        }

        [HttpGet("existsByEmail/{email}")]
        public async Task<ActionResult<bool>> ExistsByEmail(string email, CancellationToken cancellationToken)
        {
            var exists = await _responsibleService.ExistsByEmailAsync(email, cancellationToken);
            return Ok(exists);
        }

        // PAGINACIÓN
        // GET: api/responsible/paged?page=1&pageSize=10
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<ResponsibleDto>>> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _responsibleService.GetAllPagedAsync(page, pageSize, cancellationToken);
            return Ok(result);
        }
    }
}
