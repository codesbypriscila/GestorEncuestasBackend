using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{

    public class EncuestaService : IEncuestaService
    {
        private readonly AppDbContext _context;

        public EncuestaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Encuesta> CrearEncuesta(Encuesta encuesta)
        {
            _context.Encuestas.Add(encuesta);
            await _context.SaveChangesAsync();
            return encuesta;
        }

        public async Task<Encuesta> ObtenerEncuestaPorId(int id)
        {
            return await _context.Encuestas.FindAsync(id) ?? null!;
        }

        public async Task<List<Encuesta>> ListarEncuestas()
        {
            return await _context.Encuestas.ToListAsync();
        }

        public async Task<Encuesta> ActualizarEncuesta(Encuesta encuesta)
        {
            var encuestaExistente = await _context.Encuestas.FindAsync(encuesta.Id);
            if (encuestaExistente == null) return null!;

            encuestaExistente.Titulo = encuesta.Titulo;
            encuestaExistente.Descripcion = encuesta.Descripcion;
            encuestaExistente.TipoVisibilidad = encuesta.TipoVisibilidad;
            encuestaExistente.FechaExpiracion = encuesta.FechaExpiracion;

            await _context.SaveChangesAsync();
            return encuestaExistente;
        }

        public async Task<bool> EliminarEncuesta(int id)
        {
            var encuesta = await _context.Encuestas.FindAsync(id);
            if (encuesta == null) return false;

            _context.Encuestas.Remove(encuesta);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
