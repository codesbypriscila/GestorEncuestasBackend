public interface IReporteGenerator
{
    Task<string> GenerarReporteAsync(int encuestaId);
}