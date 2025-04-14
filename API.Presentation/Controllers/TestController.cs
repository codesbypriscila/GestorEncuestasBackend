using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;

namespace API.Presentation.Controllers
{
    [Route("api/database")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("test")]
        public IActionResult TestDatabaseConnection()
        {
            try
            {
                if (!_context.Database.CanConnect())
                {
                    return StatusCode(500, "❌ No se pudo conectar a la base de datos.");
                }

                return Ok("✅ Conexión a la base de datos exitosa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error en la conexión: {ex.Message}");
            }
        }
    }
}
