public class EstadisticasService : IEstadisticasService
{
    public double CalcularPromedio(List<double> respuestas)
    {
        return respuestas.Any() ? respuestas.Average() : 0.0;
    }

    public double CalcularMediana(List<double> respuestas)
    {
        if (!respuestas.Any()) return 0.0;

        var ordenadas = respuestas.OrderBy(x => x).ToList();
        int count = ordenadas.Count;

        return count % 2 == 0
            ? (ordenadas[count / 2 - 1] + ordenadas[count / 2]) / 2
            : ordenadas[count / 2];
    }

    public List<double> CalcularModa(List<double> respuestas)
    {
        if (!respuestas.Any()) return new List<double>();

        var frecuencias = respuestas
            .GroupBy(x => x)
            .ToDictionary(g => g.Key, g => g.Count());

        int maxFrecuencia = frecuencias.Values.Max();

        return frecuencias
            .Where(f => f.Value == maxFrecuencia)
            .Select(f => f.Key)
            .ToList();
    }

}
