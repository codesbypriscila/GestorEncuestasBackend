using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("Respuestas")]
public class Respuesta
{
    public int Id { get; set; }
    public int EncuestaId { get; set; }
    [JsonIgnore]
    public Encuesta? Encuesta { get; set; } 

    public int UsuarioId { get; set; }
    [JsonIgnore]
    public Usuario? Usuario { get; set; }

    public int PreguntaId { get; set; }
    [JsonIgnore]
    public Pregunta? Pregunta { get; set; }

    public DateTime FechaRespuesta { get; set; } = DateTime.Now;
    public double ValorRespuesta { get; set; } = 0.0;
}

