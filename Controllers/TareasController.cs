using GestorTarea.Application.DTOs;
using GestorTarea.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;



namespace GestorTarea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareasController : ControllerBase
    {
        private readonly GestorTareasService _servicio;

        public TareasController(GestorTareasService servicio)
        {
            _servicio = servicio;
        }

        // GET: /api/tareas
        /// <summary>
        /// "Obtiene una lista paginada y filtrada de tareas".
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="porPagina"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type =
            typeof(PaginadoResponseDto<TareaResponseDTO>))]
        public IActionResult GetAll([FromQuery] int pagina = 1, [FromQuery] int porPagina = 5, [FromQuery] string? estado = null)
        {
            var resultadoPaginado = _servicio.ObtenerPaginadas(pagina, porPagina, estado);
            return Ok(resultadoPaginado);
        }

        // GET: /api/tareas
        /// <summary>
        /// "Crear nueva tarea"
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody] TareaDTO dto)
        {
            try
            {
                var usuarioId = int.Parse(
                   User.FindFirst(ClaimTypes.NameIdentifier)!.Value
                );

                Console.WriteLine(usuarioId);
                dto.UsuarioID = usuarioId;
                _servicio.AgregarTareaDesdeDTO(dto);
                return Ok("Tarea creada correctamente");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                string err = "Error interno del servidor: ";
                return StatusCode(500, new { error = err + ex.Message });
            }

        }

        /// <summary>
        /// "Obtener una tarea por Id"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var tarea = _servicio.ObtenerTareaPorId(id);
            if (tarea == null) return NotFound(new { mensaje = "tarea no encontrada" });
            return Ok(tarea);
        }

        /// <summary>
        /// "Actualizar Tarea"
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TareaDTO dto)
        {
            var actualizada = _servicio.ActualizarTarea(id, dto);
            if (!actualizada) return NotFound(new { mensaje = "Tarea no encontrada para actualizar" });
            return Ok(new { mensaje = "Tarea actualizada correctamente" });
        }

        /// <summary>
        /// "Eliminar Tarea"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var eliminada = _servicio.EliminarTarea(id);
            if (!eliminada) return NotFound(new { mensaje = "Tara no encontrada para eliminar" });
            return Ok(new { mensaje = "Tarea eliminada correctamente" });
        }
    }
}
