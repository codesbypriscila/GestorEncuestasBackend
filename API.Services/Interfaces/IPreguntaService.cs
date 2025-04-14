public interface IPreguntaService
{
    Task<Pregunta> CrearPregunta(Pregunta pregunta);
    Task<List<Pregunta>> ObtenerPreguntasPorEncuesta(int encuestaId);
    Task<Pregunta> ActualizarPregunta(Pregunta pregunta);
    Task<Pregunta?> ObtenerPreguntaPorId(int id);
    Task<bool> EliminarPregunta(int id);
}