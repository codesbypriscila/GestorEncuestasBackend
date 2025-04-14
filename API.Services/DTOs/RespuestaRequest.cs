public class RespuestaRequest
{
    public int EncuestaId { get; set; }
    public int UsuarioId { get; set; }
    public int PreguntaId { get; set; }
    public double ValorRespuesta { get; set; } = 0.0;
}