public class Usuario
{
    public int Id {get; set;}
    public string? Username {get; set;}
    public string? Email {get; set;}
    public string? PasswordHash {get; set;}
    public string? Rol {get; set;}
    public DateTime FechaRegistro {get; set;}
}