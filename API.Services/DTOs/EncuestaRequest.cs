public class EncuestaRequest
{
    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
    public string? TipoVisibilidad { get; set; }
    public bool EstaActiva { get; set; } = true;
    public DateTime FechaExpiracion { get; set; }
}
