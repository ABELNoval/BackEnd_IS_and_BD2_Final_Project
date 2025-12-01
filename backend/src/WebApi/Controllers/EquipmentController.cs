using Application.DTOs.Equipment;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _equipmentService;

        public EquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        // ================================
        // GET: api/equipment
        // ================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _equipmentService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // ================================
        // GET: api/equipment/{id}
        // ================================
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EquipmentDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var equipment = await _equipmentService.GetByIdAsync(id, cancellationToken);
            if (equipment == null)
                return NotFound($"Equipment with id {id} not found.");

            return Ok(equipment);
        }

        // ================================
        // POST: api/equipment
        // ================================
        [HttpPost]
        public async Task<ActionResult<EquipmentDto>> Create(CreateEquipmentDto dto, CancellationToken cancellationToken)
        {
            var created = await _equipmentService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ================================
        // PUT: api/equipment/{id}
        // ================================
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EquipmentDto>> Update(Guid id, UpdateEquipmentDto dto, CancellationToken cancellationToken)
        {
            if (id != dto.Id)
                return BadRequest("ID in URL does not match ID in body.");

            var updated = await _equipmentService.UpdateAsync(dto, cancellationToken);
            if (updated == null)
                return NotFound($"Equipment with id {id} not found.");

            return Ok(updated);
        }

        // ================================
        // DELETE: api/equipment/{id}
        // ================================
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _equipmentService.DeleteAsync(id, cancellationToken);
            if (!deleted)
                return NotFound($"Equipment with id {id} not found.");

            return NoContent();
        }

        // ===========================================================
        // 🔍 EXTRA ENDPOINTS (OPCIONALES PERO MUY ÚTILES)
        // ===========================================================

        [HttpGet("byName/{name}")]
        public async Task<ActionResult<EquipmentDto>> GetByName(string name, CancellationToken cancellationToken)
        {
            var result = await _equipmentService.GetByNameAsync(name, cancellationToken);
            if (result == null)
                return NotFound($"No equipment found with name '{name}'.");

            return Ok(result);
        }

        [HttpGet("type/{typeId:guid}")]
        public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetByType(Guid typeId, CancellationToken cancellationToken)
        {
            return Ok(await _equipmentService.GetByEquipmentTypeIdAsync(typeId, cancellationToken));
        }

        [HttpGet("department/{departmentId:guid}")]
        public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetByDepartment(Guid departmentId, CancellationToken cancellationToken)
        {
            return Ok(await _equipmentService.GetByDepartmentIdAsync(departmentId, cancellationToken));
        }

        [HttpGet("state/{stateId:int}")]
        public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetByState(int stateId, CancellationToken cancellationToken)
        {
            return Ok(await _equipmentService.GetByStateAsync(stateId, cancellationToken));
        }

        [HttpGet("location/{locationTypeId:int}")]
        public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetByLocationType(int locationTypeId, CancellationToken cancellationToken)
        {
            return Ok(await _equipmentService.GetByLocationTypeAsync(locationTypeId, cancellationToken));
        }

        // ================================
        // PAGINACIÓN
        // GET: api/equipment/paged?page=1&pageSize=10
        // ================================
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return Ok(await _equipmentService.GetAllPagedAsync(page, pageSize, cancellationToken));
        }
    }
}
