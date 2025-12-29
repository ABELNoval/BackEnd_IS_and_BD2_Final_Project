using Application.DTOs.Maintenance;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly IDepartmentService _departmentService;
        private readonly IEquipmentService _equipmentService;

        public MaintenanceController (IMaintenanceService maintenanceService, IDepartmentService departmentService, IEquipmentService equipmentService)
        {
            _maintenanceService = maintenanceService;
            _departmentService = departmentService;
            _equipmentService = equipmentService;
        }

        // ==============================
        // GET: api/maintenence
        // ==============================
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
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
                        return Ok(Enumerable.Empty<MaintenanceDto>());
                    }

                    var equipments = await _equipmentService.GetByDepartmentIdsAsync(departmentIds, cancellationToken);
                    var equipmentIds = equipments.Select(e => e.Id).ToList();

                    if (!equipmentIds.Any())
                    {
                        return Ok(Enumerable.Empty<MaintenanceDto>());
                    }

                    var ids = string.Join(",", equipmentIds.Select(id => $"\"{id}\""));
                    var query = $"EquipmentId in ({ids})";
                    
                    var result = await _maintenanceService.FilterAsync(query, cancellationToken);
                    return Ok(result);
                }
            }
            else if (role == "Technical")
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    var query = $"TechnicalId == \"{userId}\"";
                    var result = await _maintenanceService.FilterAsync(query, cancellationToken);
                    return Ok(result);
                }
            }

            var resultAll = await _maintenanceService.GetAllAsync(cancellationToken);
            return Ok(resultAll);
        }

        // ==============================
        // GET: api/maintenance/{id}
        // ============================== 
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _maintenanceService.GetByIdAsync(id, cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        // ==============================
        // POST: api/maintenance
        // ==============================
        [HttpPost]
        public async Task<IActionResult> Create(CreateMaintenanceDto dto, CancellationToken cancellationToken)
        {
            Console.WriteLine($"El type es: {dto.MaintenanceTypeId}");
            var created = await _maintenanceService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ==============================
        // PUT: api/maintenance/{id}
        // ==============================
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateMaintenanceDto dto, CancellationToken cancellationToken)
        {
             if (dto.Id != id)
                dto.Id = id;

            var updated = await _maintenanceService.UpdateAsync(dto, cancellationToken);
            return updated == null ? NotFound() : Ok(updated);
        }

        // =========================================
        // DELETE: api/maintenance/{id}
        // =========================================
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var success = await _maintenanceService.DeleteAsync(id, cancellationToken);
            return success ? NoContent() : NotFound();
        }

        // =========================================
        // POST: api/maintenance/{id}/complete
        // =========================================
        [HttpPost("{id:guid}/complete")]
        public async Task<IActionResult> Complete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _maintenanceService.CompleteAsync(id, cancellationToken);
            return Ok(result);
        }

        // =========================================
        // POST: api/maintenance/filter
        // =========================================
        [HttpPost("filter")]
        public async Task<IActionResult> Filter([FromBody] List<string> request, CancellationToken cancellationToken)
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
                        return Ok(Enumerable.Empty<MaintenanceDto>());
                    }

                    var equipments = await _equipmentService.GetByDepartmentIdsAsync(departmentIds, cancellationToken);
                    var equipmentIds = equipments.Select(e => e.Id).ToList();

                    if (!equipmentIds.Any())
                    {
                        return Ok(Enumerable.Empty<MaintenanceDto>());
                    }

                    var ids = string.Join(",", equipmentIds.Select(id => $"\"{id}\""));
                    var sectionFilter = $"EquipmentId in ({ids})";
                    
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

            var result = await _maintenanceService.FilterAsync(query, cancellationToken);

            return Ok(result);
        }
    }
}