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
                Activo = usuario.Activo
            };
        }
        public Usuario CrearUsuario(UsuarioDTO dto)
        {
            var nuevoUsuario = new Usuario(dto.Nombre, dto.Email, dto.Edad, dto.Activo);
            _repositorio.Agregar(nuevoUsuario);
            return nuevoUsuario;
        }
    }
}
