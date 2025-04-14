using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("Preguntas")]
public class Pregunta
{
    public int Id { get; set; }
    public int EncuestaId { get; set; }

    [Column("Pregunta")]
    public string Texto { get; set; } = string.Empty;
    
    public string TipoRespuesta { get; set; } = string.Empty;

    [JsonIgnore]
    public Encuesta? Encuesta { get; set; }
}
