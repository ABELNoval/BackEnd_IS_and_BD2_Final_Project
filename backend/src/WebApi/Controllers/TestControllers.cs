using Microsoft.AspNetCore.Mvc;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("database")]
        public IActionResult TestDatabase()
        {
            try
            {
                // Verificar que la base de datos responde
                var canConnect = _context.Database.CanConnect();
                return Ok(new { 
                    message = "✅ Conexión a la base de datos exitosa",
                    canConnect = canConnect,
                    tables = new[] { "users", "employees", "departments", "roles" }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                    error = "❌ Error conectando a la base de datos",
                    details = ex.Message 
                });
            }
        }

        [HttpGet("migrations")]
        public IActionResult CheckMigrations()
        {
            try
            {
                var pendingMigrations = _context.Database.GetPendingMigrations();
                var appliedMigrations = _context.Database.GetAppliedMigrations();
                
                return Ok(new {
                    appliedMigrations = appliedMigrations.ToArray(),
                    pendingMigrations = pendingMigrations.ToArray(),
                    hasPending = pendingMigrations.Any()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}