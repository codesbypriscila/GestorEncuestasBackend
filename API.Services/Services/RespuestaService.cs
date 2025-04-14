using API.Data;
using Microsoft.EntityFrameworkCore;

public class RespuestaService : IRespuestaService
{
    private readonly AppDbContext _context;

    public RespuestaService(AppDbContext context)
    {
        _context = context;
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
