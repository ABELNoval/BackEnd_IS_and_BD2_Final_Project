using Application.DTOs.EquipmentType;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentTypeController : ControllerBase
    {
        private readonly IEquipmentTypeService _equipmentTypeService;

        public EquipmentTypeController(IEquipmentTypeService equipmentTypeService)
        {
            _equipmentTypeService = equipmentTypeService;
        }

        // ================================
        // GET: api/equipmenttype
        // ================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentTypeDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _equipmentTypeService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // ================================
        // GET: api/equipmenttype/{id}
        // ================================
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EquipmentTypeDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var item = await _equipmentTypeService.GetByIdAsync(id, cancellationToken);
            if (item == null)
                return NotFound($"EquipmentType with id {id} not found.");

            return Ok(item);
        }

        // ================================
        // POST: api/equipmenttype
        // ================================
        [HttpPost]
        public async Task<ActionResult<EquipmentTypeDto>> Create(CreateEquipmentTypeDto dto, CancellationToken cancellationToken)
        {
            var created = await _equipmentTypeService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ================================
        // PUT: api/equipmenttype/{id}
        // ================================
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EquipmentTypeDto>> Update(Guid id, UpdateEquipmentTypeDto dto, CancellationToken cancellationToken)
        {
            if (id != dto.Id)
                return BadRequest("ID in URL does not match ID in body.");

            var updated = await _equipmentTypeService.UpdateAsync(dto, cancellationToken);
            if (updated == null)
                return NotFound($"EquipmentType with id {id} not found.");

            return Ok(updated);
        }

        // ================================
        // DELETE: api/equipmenttype/{id}
        // ================================
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _equipmentTypeService.DeleteAsync(id, cancellationToken);
            if (!deleted)
                return NotFound($"EquipmentType with id {id} not found.");

            return NoContent();
        }

        // ===========================================================
        // 🔍 EXTRA ENDPOINTS ÚTILES
        // ===========================================================

        [HttpGet("byName/{name}")]
        public async Task<ActionResult<EquipmentTypeDto>> GetByName(string name, CancellationToken cancellationToken)
        {
            var result = await _equipmentTypeService.GetByNameAsync(name, cancellationToken);
            if (result == null)
                return NotFound($"No equipment type found with name '{name}'.");

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EquipmentTypeDto>>> SearchByName([FromQuery] string term, CancellationToken cancellationToken)
        {
            var result = await _equipmentTypeService.SearchByNameAsync(term, cancellationToken);
            return Ok(result);
        }

        [HttpGet("ordered")]
        public async Task<ActionResult<IEnumerable<EquipmentTypeDto>>> GetOrdered(CancellationToken cancellationToken)
        {
            var result = await _equipmentTypeService.GetAllOrderedByNameAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}/count")]
        public async Task<ActionResult<int>> GetEquipmentCount(Guid id, CancellationToken cancellationToken)
        {
            var count = await _equipmentTypeService.GetEquipmentCountByTypeIdAsync(id, cancellationToken);
            return Ok(count);
        }

        // ================================
        // PAGINACIÓN
        // GET: api/equipmenttype/paged?page=1&pageSize=10
        // ================================
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<EquipmentTypeDto>>> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _equipmentTypeService.GetAllPagedAsync(page, pageSize, cancellationToken);
            return Ok(result);
        }
    }
}
