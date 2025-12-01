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
        // GET: api/equipment - OBTENER TODOS
        // ================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _equipmentService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // ================================
        // GET: api/equipment/{id} - OBTENER POR ID
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
        // POST: api/equipment - CREAR NUEVO
        // ================================
        [HttpPost]
        public async Task<ActionResult<EquipmentDto>> Create(CreateEquipmentDto dto, CancellationToken cancellationToken)
        {
            var created = await _equipmentService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ================================
        // PUT: api/equipment/{id} - ACTUALIZAR EXISTENTE
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
        // DELETE: api/equipment/{id} - ELIMINAR
        // ================================
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _equipmentService.DeleteAsync(id, cancellationToken);
            if (!deleted)
                return NotFound($"Equipment with id {id} not found.");

            return NoContent();
        }
    }
}