using GestorTarea.Application.DTOs;
using GestorTarea.Application.Services;
using GestorTarea.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorTarea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registro")]
        [AllowAnonymous]
        public IActionResult Registro([FromBody] RegistroDTO dto)
        {
            var resultado = _authService.Registrar(dto);
            if (resultado == null)
                return Conflict("El email ya esta registrado");
            return Ok(resultado);
        }
        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            var repuesta = _authService.Login(dto);
            return Ok(repuesta);
        }

    }
}
