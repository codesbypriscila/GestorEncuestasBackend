using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers
{
    [Route("api/preguntas")]
    [ApiController]

    public class PreguntasController : ControllerBase
    {
        private readonly IPreguntaService _preguntaService;

        public PreguntasController(IPreguntaService preguntaService)
        {
            _preguntaService = preguntaService;
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost] //crear 
        public async Task<IActionResult> CrearPregunta([FromBody] PreguntaRequest request)
        {
            if (request.TipoRespuesta != "Escala")
            {
                return BadRequest("El tipo de respuesta solo puede ser 'Escala'.");
            }
            var pregunta = new Pregunta
            {
                EncuestaId = request.EncuestaId,
                Texto = request.PreguntaTexto!,
                TipoRespuesta = request.TipoRespuesta
            };

            var creada = await _preguntaService.CrearPregunta(pregunta);
            return Ok(creada);
        }

        [HttpGet("{encuestaId}")] //a este no le pongo el authorize porque tmb lo puede hacer un usuario
        public async Task<IActionResult> ObtenerPreguntas(int encuestaId)
        {
            var preguntas = await _preguntaService.ObtenerPreguntasPorEncuesta(encuestaId);
            return Ok(preguntas);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPregunta(int id, [FromBody] PreguntaRequest preguntaRequest)
        {
            var pregunta = await _preguntaService.ObtenerPreguntaPorId(id);

            if (pregunta == null)
                return NotFound("Pregunta no encontrada.");

            pregunta.EncuestaId = preguntaRequest.EncuestaId;
            pregunta.Texto = preguntaRequest.PreguntaTexto!;
            pregunta.TipoRespuesta = preguntaRequest.TipoRespuesta;

            var preguntaActualizada = await _preguntaService.ActualizarPregunta(pregunta);

            return Ok(preguntaActualizada);
        }

        //eliminar
        //[Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPregunta(int id)
        {
            var preguntaEliminada = await _preguntaService.EliminarPregunta(id);

            if (!preguntaEliminada)
                return NotFound("Pregunta no encontrada.");

            return NoContent();
        }


    }
}
