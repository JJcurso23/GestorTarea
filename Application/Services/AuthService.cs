using GestorTarea.Application.DTOs;
using GestorTarea.Domain.Interfaces;
using GestorTarea.Domain.Entities;
using Microsoft.AspNetCore.Authentication.BearerToken;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Diagnostics.CodeAnalysis;

namespace GestorTarea.Application.Services
{
    public class AuthService
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly IConfiguration _config;

        public AuthService(IUsuarioRepositorio repositorio, IConfiguration config)
        {
            _repositorio = repositorio;
            _config = config;
        }

        public async Task<AuthResponseDTO?> Registrar(RegistroDTO dto)
        {
            if (_repositorio.ObtenerPorEmail(dto.Email) != null)
                return null;

            var usuario = new Usuario
            (
                nombre: dto.Nombre,
                email: dto.Email,
                edad: dto.Edad,
                passwordhash: BCrypt.Net.BCrypt.HashPassword(dto.Password),
                activo: false
            );

            _repositorio.Agregar(usuario);
            return GenerarToken(usuario);
        }

        public AuthResponseDTO? Login(LoginDTO dto)
        {
            var usuario = _repositorio.ObtenerPorEmail(dto.Email);
            if (usuario == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
                return null;

            return GenerarToken(usuario);
        }

        private AuthResponseDTO GenerarToken(Usuario usuario)
        {
            var expiracion = DateTime.UtcNow.AddMinutes(
                int.Parse(_config["Jwt:ExpiracionMinutos"]!));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),                
                new Claim(ClaimTypes.Role, usuario.Activo? "Admin" : "User")
            };

            var clave = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                claims: claims,
                expires: expiracion,
                signingCredentials: credenciales);

            return new AuthResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expira = expiracion
            };
        }
    }
}
