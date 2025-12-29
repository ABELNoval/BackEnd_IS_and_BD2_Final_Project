using Application.DTOs.TransferRequest;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferRequestController : ControllerBase
    {
        private readonly ITransferRequestService _transferRequestService;
        private readonly IDepartmentService _departmentService;

        public TransferRequestController(
            ITransferRequestService transferRequestService,
            IDepartmentService departmentService)
        {
            _transferRequestService = transferRequestService;
            _departmentService = departmentService;
        }

        // ==============================
        // GET: api/transferrequest
        // ==============================
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (role == "Responsible" && Guid.TryParse(userId, out var responsibleId))
            {
                var sectionIdClaim = User.FindFirst("SectionId")?.Value;
                if (Guid.TryParse(sectionIdClaim, out var sectionId))
                {
                    var departments = await _departmentService.GetBySectionIdAsync(sectionId, cancellationToken);
                    var departmentIds = departments.Select(d => d.Id).ToList();

                    var result = await _transferRequestService.GetForResponsibleAsync(
                        responsibleId, departmentIds, cancellationToken);
                    return Ok(result);
                }
            }

            // Admin/Director get all
            var resultAll = await _transferRequestService.GetAllAsync(cancellationToken);
            return Ok(resultAll);
        }

        // ==============================
        // GET: api/transferrequest/{id}
        // ==============================
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _transferRequestService.GetByIdAsync(id, cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        // ==============================
        // POST: api/transferrequest
        // ==============================
        [HttpPost]
        public async Task<IActionResult> Create(CreateTransferRequestDto dto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var requesterId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var created = await _transferRequestService.CreateAsync(dto, requesterId, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ==============================
        // POST: api/transferrequest/{id}/accept
        // ==============================
        [HttpPost("{id:guid}/accept")]
        public async Task<IActionResult> Accept(Guid id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var resolverId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var result = await _transferRequestService.AcceptAsync(id, resolverId, cancellationToken);
            return Ok(result);
        }

        // ==============================
        // POST: api/transferrequest/{id}/deny
        // ==============================
        [HttpPost("{id:guid}/deny")]
        public async Task<IActionResult> Deny(Guid id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var resolverId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var result = await _transferRequestService.DenyAsync(id, resolverId, cancellationToken);
            return Ok(result);
        }

        // ==============================
        // POST: api/transferrequest/{id}/cancel
        // ==============================
        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var requesterId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var result = await _transferRequestService.CancelAsync(id, requesterId, cancellationToken);
            return Ok(result);
        }

        // ==============================
        // POST: api/transferrequest/filter
        // ==============================
        [HttpPost("filter")]
        public async Task<IActionResult> Filter([FromBody] List<string> request, CancellationToken cancellationToken)
        {
            string query = "";

            if (request != null && request.Count > 0)
            {
                query = string.Join(" AND ", request);
            }

            var result = await _transferRequestService.FilterAsync(query, cancellationToken);
            return Ok(result);
        }
    }
}
