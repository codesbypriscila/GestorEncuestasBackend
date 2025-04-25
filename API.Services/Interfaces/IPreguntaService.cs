public interface IPreguntaService
{
    Task<Pregunta> CrearPregunta(Pregunta pregunta);
    Task<IEnumerable<Pregunta>> ObtenerPreguntasPorEncuesta(int encuestaId);
    Task<Pregunta?> ObtenerPreguntaPorId(int id);

}