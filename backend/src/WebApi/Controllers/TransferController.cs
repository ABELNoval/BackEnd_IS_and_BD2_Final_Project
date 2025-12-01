using Application.DTOs.Transfer;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransferController (ITransferService transferService)
        {
            _transferService = transferService;
        }

        // ==============================
        // GET: api/transfer
        // ==============================
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _transferService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // ==============================
        // GET: api/transfer/{id}
        // ============================== 
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _transferService.GetByIdAsync(id, cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        // ==============================
        // POST: api/transfer
        // ==============================
        [HttpPost]
        public async Task<IActionResult> Create(CreateTransferDto dto, CancellationToken cancellationToken)
        {
             var created = await _transferService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ==============================
        // PUT: api/transfer/{id}
        // ==============================
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateTransferDto dto, CancellationToken cancellationToken)
        {
             if (dto.Id != id)
                dto.Id = id;

            var updated = await _transferService.UpdateAsync(dto, cancellationToken);
            return updated == null ? NotFound() : Ok(updated);
        }

        // =========================================
        // DELETE: api/transfer/{id}
        // =========================================
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var success = await _transferService.DeleteAsync(id, cancellationToken);
            return success ? NoContent() : NotFound();
        }

        // ===== Extra endpoints =====

        [HttpGet("sourceDepartment/{departmentId}")]
        public async Task<IActionResult> GetBySourceDepartmentId(Guid departmentId, CancellationToken cancellationToken)
        {
            return Ok(await _transferService.GetBySourceDepartmentIdAsync(departmentId, cancellationToken)); 
        }

        [HttpGet("targetDepartment/{departmentId}")]
        public async Task<IActionResult> GetByTargetDepartmentId(Guid departmentId, CancellationToken cancellationToken)
        {
            return Ok(await _transferService.GetByTargetDepartmentIdAsync(departmentId, cancellationToken)); 
        }

        [HttpGet("equipment/{equipmentId}")]
        public async Task<IActionResult> GetByEquipmentId(Guid equipmentId, CancellationToken cancellationToken)
        {
            return Ok(await _transferService.GetByEquipmentIdAsync(equipmentId, cancellationToken));
        }

    }
}