using System.ComponentModel.DataAnnotations.Schema;

[Table("Encuesta")]
public class Encuesta
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
    public string? TipoVisibilidad { get; set; }
    public bool EstaActiva { get; set; } = true;
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaExpiracion { get; set; }
    public int CreadorId { get; set; }
    
    public ICollection<Pregunta>? Preguntas { get; set; }
}
