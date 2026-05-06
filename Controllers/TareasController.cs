using GestorTarea.Application.DTOs;
using GestorTarea.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;



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
        [HttpGet]
        public IActionResult GetAll()
        {
            var tareas = _servicio.ObtenerTareas();
            return Ok(tareas);
        }

        // POST: /api/tareas
        [HttpPost]
        public IActionResult Create([FromBody] TareaDTO dto)
        {
            try
            {
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
    }
}
