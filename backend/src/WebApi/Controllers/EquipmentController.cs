using Application.DTOs.Equipment;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _equipmentService;
        private readonly IDepartmentService _departmentService;
        private readonly ITransferRequestService _transferRequestService;

        public EquipmentController(
            IEquipmentService equipmentService,
            IDepartmentService departmentService,
            ITransferRequestService transferRequestService)
        {
            _equipmentService = equipmentService;
            _departmentService = departmentService;
            _transferRequestService = transferRequestService;
        }

        // ================================
        // GET: api/equipment - OBTENER TODOS (filtrado por rol)
        // ================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetAll(CancellationToken cancellationToken)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            // Responsible: equipos de departamentos de su sección + equipos de TransferRequests aceptados
            if (role == "Responsible")
            {
                var sectionIdClaim = User.FindFirst("SectionId")?.Value;
                if (Guid.TryParse(sectionIdClaim, out var sectionId))
                {
                    var departments = await _departmentService.GetBySectionIdAsync(sectionId, cancellationToken);
                    var departmentIds = departments.Select(d => d.Id).ToList();
                    var result = await _equipmentService.GetByDepartmentIdsAsync(departmentIds, cancellationToken);
                    return Ok(result);
                }
            }
            // Employee: solo equipos de su departamento
            else if (role == "Employee")
            {
                var departmentIdClaim = User.FindFirst("DepartmentId")?.Value;
                if (Guid.TryParse(departmentIdClaim, out var departmentId))
                {
                    var result = await _equipmentService.GetByDepartmentIdsAsync(new List<Guid> { departmentId }, cancellationToken);
                    return Ok(result);
                }
            }

            // Admin, Director, Technical: todos los equipos
            var resultAll = await _equipmentService.GetAllAsync(cancellationToken);
            return Ok(resultAll);
        }

        // ================================
        // GET: api/equipment/all - OBTENER TODOS SIN FILTRAR (para dropdowns)
        // ================================
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetAllUnfiltered(CancellationToken cancellationToken)
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

        // ================================
        // POST: api/equipment/filter
        // ================================
        [HttpPost("filter")]
        public async Task<ActionResult> Filter([FromBody] List<string> request, CancellationToken cancellationToken)
        {
            string query = "";

            if (request != null && request.Count > 0)
            {
                query = string.Join(" AND ", request);
            }

            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == "Responsible")
            {
                var sectionIdClaim = User.FindFirst("SectionId")?.Value;
                if (Guid.TryParse(sectionIdClaim, out var sectionId))
                {
                    var departments = await _departmentService.GetBySectionIdAsync(sectionId, cancellationToken);
                    var departmentIds = departments.Select(d => d.Id).ToList();

                    if (!departmentIds.Any())
                    {
                        return Ok(Enumerable.Empty<EquipmentDto>());
                    }

                    var ids = string.Join(",", departmentIds.Select(id => $"Guid.Parse(\"{id}\")"));
                    var sectionFilter = $"DepartmentId in ({ids})";

                    if (!string.IsNullOrEmpty(query))
                    {
                        query = $"({query}) AND {sectionFilter}";
                    }
                    else
                    {
                        query = sectionFilter;
                    }
                }
            }

            Console.WriteLine("QUERY RECIBIDA: " + query);
            var result = await _equipmentService.FilterAsync(query, cancellationToken);

            return Ok(result);
        }
    }
}