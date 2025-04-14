using API.Data;

public class ReporteService : IReporteService
{
    private readonly AppDbContext _context;
    private readonly ReporteFactory _factory;

    public ReporteService(AppDbContext context, ReporteFactory factory)
    {
        _context = context;
        _factory = factory;
    }

    public async Task<Reporte> GenerarReporteAsync(int encuestaId, int usuarioId, string formato)
    {
        var generador = _factory.CrearGenerador(formato);
        var ruta = await generador.GenerarReporteAsync(encuestaId);

        var reporte = new Reporte
        {
            EncuestaId = encuestaId,
            GeneradoPor = usuarioId,
            Formato = formato,
            RutaArchivo = ruta
        };

        _context.Add(reporte);
        await _context.SaveChangesAsync();

        return reporte;
    }
}
