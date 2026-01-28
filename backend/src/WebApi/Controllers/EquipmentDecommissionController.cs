using Application.DTOs.EquipmentDecommission;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentDecommissionController : ControllerBase
    {
        private readonly IEquipmentDecommissionService _equipmentDecommissionService;
        private readonly IDepartmentService _departmentService;
        private readonly IEquipmentService _equipmentService;
        private readonly IEmployeeService _employeeService;

        public EquipmentDecommissionController(
            IEquipmentDecommissionService equipmentDecommissionService,
            IDepartmentService departmentService,
            IEquipmentService equipmentService,
            IEmployeeService employeeService)
        {
            _equipmentDecommissionService = equipmentDecommissionService;
            _departmentService = departmentService;
            _equipmentService = equipmentService;
            _employeeService = employeeService;
        }

        // ==============================
        // GET: api/equipmentDecommission
        // ==============================
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            // Technical: only their own decommissions
            if (role == "Technical")
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    var query = $"TechnicalId == Guid.Parse(\"{userId}\")";
                    var resultT = await _equipmentDecommissionService.FilterAsync(query, cancellationToken);
                    return Ok(resultT);
                }
            }

            // Responsible: only decommissions of their CURRENT equipment (not past)
            if (role == "Responsible")
            {
                var sectionIdClaim = User.FindFirst("SectionId")?.Value;
                if (Guid.TryParse(sectionIdClaim, out var sectionId))
                {
                    var departments = await _departmentService.GetBySectionIdAsync(sectionId, cancellationToken);
                    var departmentIds = departments.Select(d => d.Id).ToList();

                    if (!departmentIds.Any())
                    {
                        return Ok(Enumerable.Empty<EquipmentDecommissionDto>());
                    }

                    var equipments = await _equipmentService.GetByDepartmentIdsAsync(departmentIds, cancellationToken);
                    var equipmentIds = equipments.Select(e => e.Id).ToList();

                    if (!equipmentIds.Any())
                    {
                        return Ok(Enumerable.Empty<EquipmentDecommissionDto>());
                    }

                    var ids = string.Join(",", equipmentIds.Select(id => $"Guid.Parse(\"{id}\")"));
                    var query = $"EquipmentId in ({ids})";

                    var resultR = await _equipmentDecommissionService.FilterAsync(query, cancellationToken);
                    return Ok(resultR);
                }
            }

            if (role == "Receptor")
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out var userGuid))
                {
                    var query = $"RecipientId == Guid.Parse(\"{userGuid}\")";
                    var resultRec = await _equipmentDecommissionService.FilterAsync(query, cancellationToken);
                    return Ok(resultRec);
                }
                return Ok(new List<EquipmentDecommissionDto>());
            }

            // Employee: only decommissions destined to their department
            if (role == "Employee" || role == "Receptor")
            {
                var departmentIdClaim = User.FindFirst("DepartmentId")?.Value;
                if (Guid.TryParse(departmentIdClaim, out var departmentId))
                {
                    var query = $"DepartmentId == Guid.Parse(\"{departmentId}\")";
                    var resultE = await _equipmentDecommissionService.FilterAsync(query, cancellationToken);
                    return Ok(resultE);
                }
            }

            var result = await _equipmentDecommissionService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // ==============================
        // GET: api/equipmentDecommission/{id}
        // ============================== 
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _equipmentDecommissionService.GetByIdAsync(id, cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        // ==============================
        // POST: api/equipmentDecommission
        // ==============================
        [HttpPost]
        public async Task<IActionResult> Create(CreateEquipmentDecommissionDto dto, CancellationToken cancellationToken)
        {
            // Auto-fill TechnicalId for Technical role
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == "Technical")
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (Guid.TryParse(userIdClaim, out var userId))
                {
                    dto.TechnicalId = userId;
                }
            }

            Console.WriteLine($"DTO received - DestinyTypeId: {dto.DestinyTypeId}, EquipmentId: {dto.EquipmentId}, TechnicalId: {dto.TechnicalId}");
            var created = await _equipmentDecommissionService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ==============================
        // PUT: api/equipmentDecommission/{id}
        // ==============================
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateEquipmentDecommissionDto dto, CancellationToken cancellationToken)
        {
            if (dto.Id != id)
                dto.Id = id;

            var updated = await _equipmentDecommissionService.UpdateAsync(dto, cancellationToken);
            return updated == null ? NotFound() : Ok(updated);
        }

        // =========================================
        // DELETE: api/equipmentDecommission/{id}
        // =========================================
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var success = await _equipmentDecommissionService.DeleteAsync(id, cancellationToken);
            return success ? NoContent() : NotFound();
        }

        // =========================================
        // POST: api/equipmentDecommission/filter
        // =========================================
        [HttpPost("filter")]
        public async Task<IActionResult> Filter([FromBody] List<string> request, CancellationToken cancellationToken)
        {
            string query = "";

            if (request != null && request.Count > 0)
            {
                query = string.Join(" AND ", request);
            }

            var result = await _equipmentDecommissionService.FilterAsync(query, cancellationToken);

            return Ok(result);
        }

        // =====================================================
        // POST: api/equipmentDecommission/{id}/release
        // =====================================================
        /// <summary>
        /// Releases equipment from warehouse to a specific department
        /// </summary>
        /// <param name="id">The decommission ID</param>
        /// <param name="dto">The release details (target department and recipient)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated decommission record</returns>
        [HttpPost("{id:guid}/release")]
        public async Task<IActionResult> ReleaseToDepartment(
            Guid id,
            [FromBody] ReleaseToDepartmentDto dto,
            CancellationToken cancellationToken)
        {
            var updated = await _equipmentDecommissionService.ReleaseToDepartmentAsync(
                id,
                dto.TargetDepartmentId,
                dto.RecipientId,
                cancellationToken);
            return Ok(updated);
        }
    }
}