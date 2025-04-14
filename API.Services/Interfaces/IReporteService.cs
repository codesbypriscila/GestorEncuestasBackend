public interface IReporteService
{
    Task<Reporte> GenerarReporteAsync(int encuestaId, int usuarioId, string formato);
}