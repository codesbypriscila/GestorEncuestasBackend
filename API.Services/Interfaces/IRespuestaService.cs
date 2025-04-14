public interface IRespuestaService
{
    Task<Respuesta> GuardarRespuesta(Respuesta respuesta);
    Task<List<Respuesta>> ObtenerRespuestasPorEncuesta(int encuestaId);
    Task<List<Respuesta>> ObtenerRespuestasPorPregunta(int preguntaId);
}