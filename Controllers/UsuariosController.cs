using Microsoft.AspNetCore.Mvc;
using GestorTarea.Application.Services;
using GestorTarea.Application.DTOs;

namespace GestorTarea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosService _servicio;

        public UsuariosController(UsuariosService servicio)
        {
            _servicio = servicio;
        }

        /// <summary>
        /// "Obtener Usuario por Id"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var usuario = _servicio.ObtenerUsuarioId(id);
            if (usuario == null) return NotFound(new { mensaje = "Usuario no encontrado" });
            return Ok(usuario);
        }

        /// <summary>
        /// "Crear Usuario"
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] UsuarioDTO dto)
        {
            _servicio.CrearUsuario(dto);
            return Ok(new { mensaje = "Usuario creado correctamente" });
        }

        /// <summary>
        /// "Obtener Usuario por Email"
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("buscar/{email}")]
        public IActionResult GetByEmail(string email)
        {
            var usuario = _servicio.ObtenerPorEmail(email);
            if (usuario == null) return NotFound(new { mensaje = "Usuario no encontrado." });
            return Ok(usuario);
        }
    }
}