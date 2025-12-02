using Application.DTOs.Maintenance;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;

        public MaintenanceController (IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        // ==============================
        // GET: api/maintenence
        // ==============================
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _maintenanceService.GetAllAsync(cancellationToken);
            return Ok(result);
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

    }
}