using Application.DTOs.Transfer;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;
        private readonly IDepartmentService _departmentService;

        public TransferController (ITransferService transferService, IDepartmentService departmentService)
        {
            _transferService = transferService;
            _departmentService = departmentService;
        }

        // ==============================
        // GET: api/transfer
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
                        return Ok(Enumerable.Empty<TransferDto>());
                    }

                    var result = await _transferService.GetByDepartmentIdsAsync(departmentIds, cancellationToken);
                    return Ok(result);
                }
            }

            var resultAll = await _transferService.GetAllAsync(cancellationToken);
            return Ok(resultAll);
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

        // =========================================
        // POST: api/transfer/filter
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
                        return Ok(Enumerable.Empty<TransferDto>());
                    }

                    var ids = string.Join(",", departmentIds.Select(id => $"\"{id}\""));
                    var sectionFilter = $"(SourceDepartmentId in ({ids}) || TargetDepartmentId in ({ids}))";
                    
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

            var result = await _transferService.FilterAsync(query, cancellationToken);

            return Ok(result);
        }
    }
}