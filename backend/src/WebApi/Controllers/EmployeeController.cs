using Application.DTOs.Employee;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        // =========================================
        // GET: api/employee
        // =========================================
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
                        return Ok(Enumerable.Empty<EmployeeDto>());
                    }

                    var ids = string.Join(",", departmentIds.Select(id => $"\"{id}\""));
                    var query = $"DepartmentId in ({ids})";
                    
                    var result = await _employeeService.FilterAsync(query, cancellationToken);
                    return Ok(result);
                }
            }

            var resultAll = await _employeeService.GetAllAsync(cancellationToken);
            return Ok(resultAll);
        }

        // =========================================
        // GET: api/employee/{id}
        // =========================================
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _employeeService.GetByIdAsync(id, cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        // =========================================
        // POST: api/employee
        // =========================================
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto, CancellationToken cancellationToken)
        {
            var created = await _employeeService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // =========================================
        // PUT: api/employee/{id}
        // =========================================
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateEmployeeDto dto, CancellationToken cancellationToken)
        {
            if (dto.Id != id)
                dto.Id = id;

            var updated = await _employeeService.UpdateAsync(dto, cancellationToken);
            return updated == null ? NotFound() : Ok(updated);
        }

        // =========================================
        // DELETE: api/employee/{id}
        // =========================================
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var success = await _employeeService.DeleteAsync(id, cancellationToken);
            return success ? NoContent() : NotFound();
        }

        // =========================================
        // POST: api/employee/filter
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
                        return Ok(Enumerable.Empty<EmployeeDto>());
                    }

                    var ids = string.Join(",", departmentIds.Select(id => $"\"{id}\""));
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

            var result = await _employeeService.FilterAsync(query, cancellationToken);

            return Ok(result);
        }
    }
}
