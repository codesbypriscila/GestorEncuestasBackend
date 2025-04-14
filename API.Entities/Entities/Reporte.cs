using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("Reportes")]
public class Reporte
{
    public int Id { get; set; }
    public int EncuestaId { get; set; }
    [JsonIgnore]
    public Encuesta? Encuesta { get; set; }

    public int GeneradoPor { get; set; }
    [JsonIgnore]
    public Usuario? Usuario { get; set; }

    public DateTime FechaGenerado { get; set; } = DateTime.Now;
    public string Formato { get; set; } = string.Empty; 
    public string RutaArchivo { get; set; } = string.Empty;
}

