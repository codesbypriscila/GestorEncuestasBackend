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

    public async Task<IEnumerable<Pregunta>> ObtenerPreguntasPorEncuesta(int encuestaId)
    {
        return await _context.Preguntas
            .Where(p => p.EncuestaId == encuestaId)
            .ToListAsync();
    }

    public async Task<Pregunta?> ObtenerPreguntaPorId(int id)
    {
        return await _context.Preguntas.FindAsync(id);
    }


}