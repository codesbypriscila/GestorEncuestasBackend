using API.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

public class PreguntaService : IPreguntaService
{
    private readonly AppDbContext _context;

    public PreguntaService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Pregunta> CrearPregunta(Pregunta pregunta)
    {
        _context.Preguntas.Add(pregunta);
        await _context.SaveChangesAsync();
        return pregunta;
    }

    public async Task<List<Pregunta>> ObtenerPreguntasPorEncuesta(int encuestaId)
    {
        return await _context.Preguntas
        .Where(p => p.EncuestaId == encuestaId)
        .ToListAsync();
    }

    public async Task<Pregunta> ActualizarPregunta(Pregunta pregunta)
    {
        var preguntaExistente = await _context.Preguntas.FindAsync(pregunta.Id);
        if (preguntaExistente == null) return null!;

        preguntaExistente.EncuestaId = pregunta.EncuestaId;
        preguntaExistente.Texto = pregunta.Texto;
        preguntaExistente.TipoRespuesta = pregunta.TipoRespuesta;

        await _context.SaveChangesAsync();
        return preguntaExistente;
    }

    public async Task<Pregunta?> ObtenerPreguntaPorId(int id)
    {
        return await _context.Preguntas.FindAsync(id);
    }

    public async Task<bool> EliminarPregunta(int id)
    {
        var pregunta = await _context.Preguntas.FindAsync(id);
        if (pregunta == null) return false;

        _context.Preguntas.Remove(pregunta);
        await _context.SaveChangesAsync();
        return true;
    }

}