using System.ComponentModel.DataAnnotations;

public class PreguntaRequest
{
    public int EncuestaId { get; set; }
    public string? PreguntaTexto { get; set; }

    [Required]
    [RegularExpression("Escala", ErrorMessage = "Solo se permite el tipo de respuesta 'Escala'.")]
    public string TipoRespuesta { get; set; } = string.Empty;
}