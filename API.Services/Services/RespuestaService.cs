using API.Data;
using Microsoft.EntityFrameworkCore;

public class RespuestaService : IRespuestaService
{
    private readonly AppDbContext _context;
    private readonly EmailService _emailService;

    public RespuestaService(AppDbContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<Respuesta> GuardarRespuesta(Respuesta respuesta)
    {
        var yaRespondida = await _context.Respuestas.AnyAsync(r =>
            r.UsuarioId == respuesta.UsuarioId &&
            r.PreguntaId == respuesta.PreguntaId);

        if (yaRespondida)
        {
            return null!;
        }

        _context.Respuestas.Add(respuesta);
        await _context.SaveChangesAsync();

        var nombreEncuesta = await _context.Encuestas
            .Where(e => e.Id == respuesta.EncuestaId)
            .Select(e => e.Titulo)
            .FirstOrDefaultAsync();

        await _emailService.EnviarCorreoNotificacion(nombreEncuesta!, respuesta.UsuarioId);

        return respuesta;
    }

    public async Task<List<Respuesta>> ObtenerRespuestasPorEncuesta(int encuestaId)
    {
        return await _context.Respuestas
            .Where(r => r.EncuestaId == encuestaId)
            .ToListAsync();
    }

    public async Task<List<Respuesta>> ObtenerRespuestasPorPregunta(int preguntaId)
    {
        return await _context.Respuestas
            .Where(r => r.PreguntaId == preguntaId)
            .ToListAsync();
    }
}
