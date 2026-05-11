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

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var usuario = _servicio.ObtenerUsuarioId(id);
            if (usuario == null) return NotFound(new { mensaje = "Usuario no encontrado" });
            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult Create([FromBody] UsuarioDTO dto)
        {
            _servicio.CrearUsuario(dto);
            return Ok(new { mensaje = "Usuario creado correctamente" });
        }
    }
}