using API.Data;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

public class ExcelReporteGenerator : IReporteGenerator
{
    private readonly AppDbContext _context;
    private readonly IEstadisticasService _estadisticasService;

    public ExcelReporteGenerator(AppDbContext context, IEstadisticasService estadisticasService)
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
            .FirstOrDefaultAsync(r => r.EncuestaId == encuestaId && r.Formato.ToLower() == "excel");

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

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Reporte de Encuesta");

            worksheet.Cells[1, 1].Value = $"Reporte de Encuesta: {encuesta.Titulo}";
            worksheet.Cells[2, 1].Value = $"Fecha de generación: {DateTime.Now}";

            int row = 4;
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

                    worksheet.Cells[row, 1].Value = $"Pregunta: {pregunta.Texto}";
                    worksheet.Cells[row + 1, 1].Value = $"Promedio: {promedio}";
                    worksheet.Cells[row + 2, 1].Value = $"Mediana: {mediana}";
                    worksheet.Cells[row + 3, 1].Value = $"Moda: {string.Join(", ", moda)}";
                    row += 5;
                }
            }

            string nombreArchivo = $"Reporte_Encuesta_{encuestaId}_{DateTime.Now.Ticks}.xlsx";
            ruta = Path.Combine("Reportes", nombreArchivo);

            Directory.CreateDirectory("Reportes");

            await File.WriteAllBytesAsync(ruta, package.GetAsByteArray());

            reporteExistente = new Reporte
            {
                EncuestaId = encuestaId,
                Formato = "excel",
                RutaArchivo = ruta,
                FechaGenerado = DateTime.Now,
                GeneradoPor = 1 
            };

            _context.Reportes.Add(reporteExistente);
            await _context.SaveChangesAsync();
        }

        return ruta;
    }
}
