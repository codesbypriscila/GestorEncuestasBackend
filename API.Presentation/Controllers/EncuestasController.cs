using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Presentation.Controllers
{
    [Route("api/encuestas")]
    [ApiController]
    public class EncuestasController : ControllerBase
    {
        private readonly IEncuestaService _encuestaService;

        public EncuestasController(IEncuestaService encuestaService)
        {
            _encuestaService = encuestaService;
        }

        //crear
        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> CrearEncuesta([FromBody] EncuestaRequest encuestaRequest)
        {
            if (encuestaRequest == null)
                return BadRequest("Los datos de la encuesta son inválidos.");

            var encuesta = new Encuesta
            {
                Titulo = encuestaRequest.Titulo,
                Descripcion = encuestaRequest.Descripcion,
                TipoVisibilidad = encuestaRequest.TipoVisibilidad,
                EstaActiva = encuestaRequest.EstaActiva,
                FechaExpiracion = encuestaRequest.FechaExpiracion,
                FechaCreacion = DateTime.Now,
                CreadorId = 1
            };

            var encuestaCreada = await _encuestaService.CrearEncuesta(encuesta);

            if (encuestaCreada == null)
                return BadRequest("No se pudo crear la tarea.");

            return CreatedAtAction(nameof(GetEncuesta), new { id = encuestaCreada.Id }, encuestaCreada);
        }

        //encuesta por id
        //[Authorize(Roles = "Administrador")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEncuesta(int id)
        {
            var encuesta = await _encuestaService.ObtenerEncuestaPorId(id);

            if (encuesta == null)
                return NotFound("Tarea no encontrada.");

            return Ok(encuesta);
        }

        //listar y buscar
        [HttpGet] //a este no le pongo el authorize porque tmb lo puede hacer un usuario
        public async Task<IActionResult> ListarEncuestas()
        {
            var encuestas = await _encuestaService.ListarEncuestas();
            return Ok(encuestas);
        }

        //actualizar
        //[Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEncuesta(int id, [FromBody] EncuestaRequest encuestaRequest)
        {
            var encuesta = await _encuestaService.ObtenerEncuestaPorId(id);

            if (encuesta == null)
                return NotFound($"Encuesta con ID {id} no encontrada.");

            encuesta.Titulo = encuestaRequest.Titulo;
            encuesta.Descripcion = encuestaRequest.Descripcion;
            encuesta.TipoVisibilidad = encuestaRequest.TipoVisibilidad;
            encuesta.EstaActiva = encuestaRequest.EstaActiva;
            encuesta.FechaExpiracion = encuestaRequest.FechaExpiracion;

        try
        {
            var encuestaActualizada = await _encuestaService.ActualizarEncuesta(encuesta);
            if (encuestaActualizada == null)
            {
                return BadRequest("Hubo un problema al actualizar la encuesta.");
            }

            return Ok(encuestaActualizada);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
}


        //eliminar
        //[Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEncuesta(int id)
        {
            var encuestaEliminada = await _encuestaService.EliminarEncuesta(id);

            if (!encuestaEliminada)
                return NotFound("Encuesta no encontrada.");

            return NoContent();
        }
    }
}