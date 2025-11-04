using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TechnicalDowntimeController : ControllerBase
    {
        private readonly TechnicalDowntimeService _service;

        public TechnicalDowntimeController(TechnicalDowntimeService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTechnicalDowntime([FromBody] TechnicalDowntimeDTO dto)
        {
            await _service.CreateTechnicalDowntimeAsync(dto);
            return Ok("Baja técnica registrada exitosamente");
        }
    }
}