using GestorTarea.Application.DTOs;
using GestorTarea.Domain.Entities;
using GestorTarea.Infrastructure.Repositories;
using System;

namespace GestorTarea.Application.Services
{
    public class UsuariosService
    {
        private readonly UsuarioRepositorio _repositorio;

        public UsuariosService(UsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        public UsuarioDTO? ObtenerUsuarioId(int id)
        {
            var usuario = _repositorio.ObtenerPorId(id);
            if (usuario == null) return null;

            return new UsuarioDTO
            {
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Edad = usuario.Edad,
                Activo = usuario.Activo,
                Password = usuario.PasswordHash
            };
        }
        public UsuarioDTO ObtenerPorEmail(string email)
        {
            var usuario = _repositorio.ObtenerPorEmail(email);
            if (usuario == null) return null;

            return new UsuarioDTO
            {
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Edad = usuario.Edad,
                Activo = usuario.Activo,
                Password = usuario.PasswordHash
            };
        }
        public Usuario CrearUsuario(UsuarioDTO dto)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var nuevoUsuario = new Usuario(dto.Nombre, dto.Email, dto.Edad, hash, dto.Activo);
            _repositorio.Agregar(nuevoUsuario);
            return nuevoUsuario;
        }
    }
}
