using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers
{
    [Route("api/respuestas")]
    [ApiController]
    public class RespuestasController : ControllerBase
    {
        private readonly IRespuestaService _respuestaService;

        public RespuestasController(IRespuestaService respuestaService)
        {
            _respuestaService = respuestaService;
        }

        [HttpPost]
        public async Task<IActionResult> GuardarRespuesta([FromBody] RespuestaRequest request)
        {
            var respuesta = new Respuesta
            {
                EncuestaId = request.EncuestaId,
                UsuarioId = request.UsuarioId,
                PreguntaId = request.PreguntaId,
                ValorRespuesta = request.ValorRespuesta
            };

            var guardada = await _respuestaService.GuardarRespuesta(respuesta);

            if (guardada == null)
                return BadRequest("El usuario ya ha respondido esta pregunta.");
            return Ok(guardada);
        }

        [HttpGet("encuesta/{encuestaId}")]
        public async Task<IActionResult> ObtenerRespuestasPorEncuesta(int encuestaId)
        {
            var respuestas = await _respuestaService.ObtenerRespuestasPorEncuesta(encuestaId);
            return Ok(respuestas);
        }

        [HttpGet("pregunta/{preguntaId}")]
        public async Task<IActionResult> ObtenerRespuestasPorPregunta(int preguntaId)
        {
            var respuestas = await _respuestaService.ObtenerRespuestasPorPregunta(preguntaId);
            return Ok(respuestas);
        }
    }
}
