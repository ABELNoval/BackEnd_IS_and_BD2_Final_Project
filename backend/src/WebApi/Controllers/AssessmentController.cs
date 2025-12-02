using Application.DTOs.Assessment;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssessmentController : ControllerBase
    {
        private readonly IAssessmentService _assessmentService;

        public AssessmentController (IAssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
        }

        // ==============================
        // GET: api/assessment
        // ==============================
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _assessmentService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        // ==============================
        // GET: api/assessment/{id}
        // ============================== 
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _assessmentService.GetByIdAsync(id, cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        // ==============================
        // POST: api/assessment
        // ==============================
        [HttpPost]
        public async Task<IActionResult> Create(CreateAssessmentDto dto, CancellationToken cancellationToken)
        {
             var created = await _assessmentService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ==============================
        // PUT: api/assessment/{id}
        // ==============================
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateAssessmentDto dto, CancellationToken cancellationToken)
        {
             if (dto.Id != id)
                dto.Id = id;

            var updated = await _assessmentService.UpdateAsync(dto, cancellationToken);
            return updated == null ? NotFound() : Ok(updated);
        }

        // =========================================
        // DELETE: api/assessment/{id}
        // =========================================
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var success = await _assessmentService.DeleteAsync(id, cancellationToken);
            return success ? NoContent() : NotFound();
        }

        // ===== Extra endpoints =====

        [HttpGet("director/{directorId}")]
        public async Task<IActionResult> GeByDirectorId(Guid directorId, CancellationToken cancellationToken)
        {
            return Ok(await _assessmentService.GetByDirectorIdAsync(directorId, cancellationToken));
        }

        [HttpGet("technical/{technicalId}")]
        public async Task<IActionResult> GeByTechnicalId(Guid technicalId, CancellationToken cancellationToken)
        {
            return Ok(await _assessmentService.GetByTechnicalIdAsync(technicalId, cancellationToken));
        }
    }
}