using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/reportes")]
public class ReportesController : ControllerBase
{
    private readonly IReporteService _reporteService;

    public ReportesController(IReporteService reporteService)
    {
        _reporteService = reporteService;
    }

    [HttpPost]
    public async Task<IActionResult> GenerarReporte([FromBody] ReporteRequest request)
    {
        try
        {
            var reporte = await _reporteService.GenerarReporteAsync(
                request.EncuestaId, request.UsuarioId, request.Formato);

            return Ok(reporte);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
