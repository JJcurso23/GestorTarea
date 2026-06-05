using GestorTarea.Application.DTOs;
using GestorTarea.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;



namespace GestorTarea.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TareasController : ControllerBase
    {
        private readonly GestorTareasService _servicio;

        public TareasController(GestorTareasService servicio)
        {
            _servicio = servicio;
        }

        private int UsuarioActualId =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        private bool EsAdmin =>
            User.IsInRole("Admin");

        /// <summary>
        /// "Obtiene una lista paginada y filtrada de tareas. Admin ve todas, usuario solo las suyas".
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type =
            typeof(PaginadoResponseDto<TareaResponseDTO>))]
        public IActionResult GetAll([FromQuery] int pagina = 1, [FromQuery] int porPagina = 5, [FromQuery] string? estado = null)
        {
            var resultadoPaginado = _servicio.ObtenerPaginadas(pagina, porPagina, estado, UsuarioActualId, EsAdmin);
            return Ok(resultadoPaginado);
        }

        /// <summary>
        /// "Crear nueva tarea"
        /// </summary>
        [HttpPost]
        public IActionResult Create([FromBody] TareaDTO dto)
        {
            try
            {
                dto.UsuarioID = UsuarioActualId;
                _servicio.AgregarTareaDesdeDTO(dto);
                return Ok("Tarea creada correctamente");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor: " + ex.Message });
            }
        }

        /// <summary>
        /// "Obtener una tarea por Id"
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var tarea = _servicio.ObtenerTareaPorId(id, UsuarioActualId, EsAdmin);
            if (tarea == null) return NotFound(new { mensaje = "Tarea no encontrada" });
            return Ok(tarea);
        }

        /// <summary>
        /// "Actualizar Tarea"
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TareaDTO dto)
        {
            var resultado = _servicio.ActualizarTarea(id, dto, UsuarioActualId, EsAdmin);
            return resultado switch
            {
                GestorTareasService.ResultadoOperacion.Ok => Ok(new { mensaje = "Tarea actualizada correctamente" }),
                GestorTareasService.ResultadoOperacion.NoEncontrada => NotFound(new { mensaje = "Tarea no encontrada" }),
                GestorTareasService.ResultadoOperacion.Prohibida => Forbid(),
                _ => StatusCode(500)
            };
        }

        /// <summary>
        /// "Marcar tarea como completada"
        /// </summary>
        [HttpPost("{id}/completar")]
        public IActionResult Completar(int id)
        {
            var resultado = _servicio.CompletarTarea(id, UsuarioActualId, EsAdmin);
            return resultado switch
            {
                GestorTareasService.ResultadoOperacion.Ok => Ok(new { mensaje = "Tarea completada" }),
                GestorTareasService.ResultadoOperacion.NoEncontrada => NotFound(new { mensaje = "Tarea no encontrada" }),
                GestorTareasService.ResultadoOperacion.Prohibida => Forbid(),
                _ => StatusCode(500)
            };
        }

        /// <summary>
        /// "Iniciar una sesion de Pomodoro: marca TiempoInit/TiempoFinal en la tarea"
        /// </summary>
        [HttpPost("{id}/pomodoro/iniciar")]
        public IActionResult IniciarPomodoro(int id)
        {
            var resultado = _servicio.IniciarPomodoro(id, UsuarioActualId, EsAdmin);
            return resultado switch
            {
                GestorTareasService.ResultadoOperacion.Ok => Ok(new { mensaje = "Pomodoro iniciado" }),
                GestorTareasService.ResultadoOperacion.NoEncontrada => NotFound(new { mensaje = "Tarea no encontrada" }),
                GestorTareasService.ResultadoOperacion.Prohibida => Forbid(),
                _ => StatusCode(500)
            };
        }

        /// <summary>
        /// "Registrar una sesion de Pomodoro como completada (incrementa contador)"
        /// </summary>
        [HttpPost("{id}/pomodoro/completar-sesion")]
        public IActionResult CompletarSesionPomodoro(int id)
        {
            var resultado = _servicio.CompletarSesionPomodoro(id, UsuarioActualId, EsAdmin);
            return resultado switch
            {
                GestorTareasService.ResultadoOperacion.Ok => Ok(new { mensaje = "Sesion registrada" }),
                GestorTareasService.ResultadoOperacion.NoEncontrada => NotFound(new { mensaje = "Tarea no encontrada" }),
                GestorTareasService.ResultadoOperacion.Prohibida => Forbid(),
                _ => StatusCode(500)
            };
        }

        /// <summary>
        /// "Reabrir una tarea (vuelve a Pendiente)"
        /// </summary>
        [HttpPost("{id}/reabrir")]
        public IActionResult Reabrir(int id)
        {
            var resultado = _servicio.ReabrirTarea(id, UsuarioActualId, EsAdmin);
            return resultado switch
            {
                GestorTareasService.ResultadoOperacion.Ok => Ok(new { mensaje = "Tarea reabierta" }),
                GestorTareasService.ResultadoOperacion.NoEncontrada => NotFound(new { mensaje = "Tarea no encontrada" }),
                GestorTareasService.ResultadoOperacion.Prohibida => Forbid(),
                _ => StatusCode(500)
            };
        }

        public class CancelarDto { public string? Motivo { get; set; } }

        /// <summary>
        /// "Descartar tarea (pasa a Cancelada)"
        /// </summary>
        [HttpPost("{id}/cancelar")]
        public IActionResult Cancelar(int id, [FromBody] CancelarDto? dto)
        {
            var resultado = _servicio.CancelarTarea(id, dto?.Motivo, UsuarioActualId, EsAdmin);
            return resultado switch
            {
                GestorTareasService.ResultadoOperacion.Ok => Ok(new { mensaje = "Tarea cancelada" }),
                GestorTareasService.ResultadoOperacion.NoEncontrada => NotFound(new { mensaje = "Tarea no encontrada" }),
                GestorTareasService.ResultadoOperacion.Prohibida => Forbid(),
                _ => StatusCode(500)
            };
        }

        /// <summary>
        /// "Eliminar Tarea"
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var resultado = _servicio.EliminarTarea(id, UsuarioActualId, EsAdmin);
            return resultado switch
            {
                GestorTareasService.ResultadoOperacion.Ok => Ok(new { mensaje = "Tarea eliminada correctamente" }),
                GestorTareasService.ResultadoOperacion.NoEncontrada => NotFound(new { mensaje = "Tarea no encontrada" }),
                GestorTareasService.ResultadoOperacion.Prohibida => Forbid(),
                _ => StatusCode(500)
            };
        }
    }
}
