using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task EnviarCorreoNotificacion(string nombreEncuesta, int usuarioId)
    {
        var email = _config["EmailSettings:Email"];
        var password = _config["EmailSettings:Password"];
        var smtpServer = _config["EmailSettings:SmtpServer"];
        var port = int.Parse(_config["EmailSettings:Port"]!);

        var mensaje = new MimeMessage();
        mensaje.From.Add(new MailboxAddress("Plataforma Encuestas", email));
        mensaje.To.Add(new MailboxAddress("Administrador", email));
        mensaje.Subject = "Nueva encuesta respondida";
        mensaje.Body = new TextPart("plain")
        {
            Text = $"El usuario con ID {usuarioId} respondió la encuesta '{nombreEncuesta}'."
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpServer, port, false);
        await client.AuthenticateAsync(email, password);
        await client.SendAsync(mensaje);
        await client.DisconnectAsync(true);
    }
}
