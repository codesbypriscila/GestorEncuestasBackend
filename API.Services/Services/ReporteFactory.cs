using Microsoft.Extensions.DependencyInjection;

public class ReporteFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ReporteFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IReporteGenerator CrearGenerador(string formato)
    {
        return formato.ToLower() switch
        {
            "pdf" => _serviceProvider.GetRequiredService<PdfReporteGenerator>(),
            "excel" => _serviceProvider.GetRequiredService<ExcelReporteGenerator>(),
            _ => throw new ArgumentException("Formato no válido", nameof(formato)),
        };
    }
}
