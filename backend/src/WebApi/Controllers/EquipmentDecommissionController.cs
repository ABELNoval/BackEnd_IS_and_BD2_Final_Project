using Application.DTOs.EquipmentDecommission;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentDecommissionController : ControllerBase
    {
        private readonly IEquipmentDecommissionService _equipmentDecommissionService;

        public EquipmentDecommissionController (IEquipmentDecommissionService equipmentDecommissionService)
        {
            _equipmentDecommissionService = equipmentDecommissionService;
        }

        // ==============================
        // GET: api/equipmentDecommission
        // ==============================
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
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
    }
}