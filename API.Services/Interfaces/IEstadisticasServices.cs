public interface IEstadisticasService
{
    double CalcularPromedio(List<double> respuestas);
    double CalcularMediana(List<double> respuestas);
    List<double> CalcularModa(List<double> respuestas);

}
