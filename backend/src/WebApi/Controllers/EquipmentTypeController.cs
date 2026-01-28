using Application.DTOs.EquipmentType;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentTypeController : ControllerBase
    {
        private readonly IEquipmentTypeService _equipmentTypeService;
        private readonly IEquipmentService _equipmentService;
        private readonly IDepartmentService _departmentService;
        private readonly ITransferRequestService _transferRequestService;

        public EquipmentTypeController(
            IEquipmentTypeService equipmentTypeService,
            IEquipmentService equipmentService,
            IDepartmentService departmentService,
            ITransferRequestService transferRequestService)
        {
            _equipmentTypeService = equipmentTypeService;
            _equipmentService = equipmentService;
            _departmentService = departmentService;
            _transferRequestService = transferRequestService;
        }

        // ================================
        // GET: api/equipmenttype - OBTENER TODOS (filtrado por rol)
        // ================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentTypeDto>>> GetAll(CancellationToken cancellationToken)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Responsible: equipment types from their visible equipment (current + past)
            if (role == "Responsible")
            {
                var sectionIdClaim = User.FindFirst("SectionId")?.Value;
                if (Guid.TryParse(sectionIdClaim, out var sectionId))
                {
                    var departments = await _departmentService.GetBySectionIdAsync(sectionId, cancellationToken);
                    var departmentIds = departments.Select(d => d.Id).ToList();
                    var equipments = await _equipmentService.GetByDepartmentIdsAsync(departmentIds, cancellationToken);
                    
                    var typeIds = equipments.Select(e => e.EquipmentTypeId).Distinct().ToList();
                    var allTypes = await _equipmentTypeService.GetAllAsync(cancellationToken);
                    var filteredTypes = allTypes.Where(t => typeIds.Contains(t.Id)).ToList();
                    return Ok(filteredTypes);
                }
            }
            
            // Employee: only equipment types from their department's equipment
            if (role == "Employee" || role == "Receptor")
            {
                var departmentIdClaim = User.FindFirst("DepartmentId")?.Value;
                if (Guid.TryParse(departmentIdClaim, out var departmentId))
                {
                    // Get equipment in their department
                    var equipments = await _equipmentService.GetByDepartmentIdsAsync(
                        new List<Guid> { departmentId }, cancellationToken);
                    
                    // Get unique equipment type IDs
                    var typeIds = equipments.Select(e => e.EquipmentTypeId).Distinct().ToList();
                    
                    // Get only those equipment types
                    var allTypes = await _equipmentTypeService.GetAllAsync(cancellationToken);
                    var filteredTypes = allTypes.Where(t => typeIds.Contains(t.Id)).ToList();
                    return Ok(filteredTypes);
                }
            }
            
            var result = await _equipmentTypeService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // ================================
        // GET: api/equipmenttype/{id} - OBTENER POR ID
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
        // POST: api/equipmenttype - CREAR NUEVO
        // ================================
        [HttpPost]
        public async Task<ActionResult<EquipmentTypeDto>> Create(CreateEquipmentTypeDto dto, CancellationToken cancellationToken)
        {
            var created = await _equipmentTypeService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ================================
        // PUT: api/equipmenttype/{id} - ACTUALIZAR EXISTENTE
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
        // DELETE: api/equipmenttype/{id} - ELIMINAR
        // ================================
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _equipmentTypeService.DeleteAsync(id, cancellationToken);
            if (!deleted)
                return NotFound($"EquipmentType with id {id} not found.");

            return NoContent();
        }

        // ================================
        // POST: api/equipmenttype/filter
        // ================================
        [HttpPost("filter")]
        public async Task<ActionResult> Filter([FromBody] List<string> request, CancellationToken cancellationToken)
        {
            string query = "";

            if (request != null && request.Count > 0)
            {
                query = string.Join(" AND ", request);
            }

            var result = await _equipmentTypeService.FilterAsync(query, cancellationToken);

            return Ok(result);
        }
    }
}