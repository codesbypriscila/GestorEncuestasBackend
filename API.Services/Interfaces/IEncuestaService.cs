public interface IEncuestaService
{
    Task<Encuesta> CrearEncuesta(Encuesta encuesta);
    Task<Encuesta> ObtenerEncuestaPorId(int id);
    Task<List<Encuesta>> ListarEncuestas();
    Task<Encuesta> ActualizarEncuesta(Encuesta encuesta);
    Task<bool> EliminarEncuesta(int id);
}