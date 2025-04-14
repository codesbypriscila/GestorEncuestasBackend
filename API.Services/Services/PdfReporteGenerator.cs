using API.Data;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class PdfReporteGenerator : IReporteGenerator
{
    private readonly AppDbContext _context;
    private readonly IEstadisticasService _estadisticasService;

    public PdfReporteGenerator(AppDbContext context, IEstadisticasService estadisticasService)
    {
        _context = context;
        _estadisticasService = estadisticasService;
    }

    public async Task<string> GenerarReporteAsync(int encuestaId)
    {
        var encuesta = await _context.Encuestas.FindAsync(encuestaId);
        if (encuesta == null)
            throw new ArgumentException("Encuesta no encontrada");

        var reporteExistente = await _context.Reportes
            .FirstOrDefaultAsync(r => r.EncuestaId == encuestaId && r.Formato.ToLower() == "pdf");

        string ruta;

        if (reporteExistente != null)
        {
            return reporteExistente.RutaArchivo;
        }

        var preguntas = await _context.Preguntas
            .Where(p => p.EncuestaId == encuestaId)
            .ToListAsync();

        var respuestas = await _context.Respuestas
            .Where(r => r.EncuestaId == encuestaId)
            .ToListAsync();

        string nombreArchivo = $"Reporte_Encuesta_{encuestaId}_{DateTime.Now.Ticks}.pdf";
        ruta = Path.Combine("Reportes", nombreArchivo);

        Directory.CreateDirectory("Reportes");

        using (var writer = new PdfWriter(ruta))
        using (var pdf = new PdfDocument(writer))
        {
            var document = new Document(pdf);

            document.Add(new Paragraph($"Reporte de Encuesta: {encuesta.Titulo}"));
            document.Add(new Paragraph($"Fecha de generación: {DateTime.Now}"));
            document.Add(new Paragraph(""));

            foreach (var pregunta in preguntas)
            {
                var respuestasPregunta = respuestas
                    .Where(r => r.PreguntaId == pregunta.Id)
                    .Select(r => r.ValorRespuesta)
                    .ToList();

                if (respuestasPregunta.Any())
                {
                    double promedio = _estadisticasService.CalcularPromedio(respuestasPregunta);
                    double mediana = _estadisticasService.CalcularMediana(respuestasPregunta);
                    var moda = _estadisticasService.CalcularModa(respuestasPregunta);

                    document.Add(new Paragraph($"Pregunta: {pregunta.Texto}"));
                    document.Add(new Paragraph($"Promedio: {promedio}"));
                    document.Add(new Paragraph($"Mediana: {mediana}"));
                    document.Add(new Paragraph($"Moda: {string.Join(", ", moda)}"));
                    document.Add(new Paragraph(""));
                }
            }
        }

        reporteExistente = new Reporte
        {
            EncuestaId = encuestaId,
            Formato = "pdf",
            RutaArchivo = ruta,
            FechaGenerado = DateTime.Now,
            GeneradoPor = 1 
        };

        _context.Reportes.Add(reporteExistente);
        await _context.SaveChangesAsync();

        return ruta;
    }
}
