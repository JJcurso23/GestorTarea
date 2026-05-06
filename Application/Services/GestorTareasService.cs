using System;
using System.Collections.Generic;
using System.Linq;
using GestorTarea.Domain.Interfaces;
using GestorTarea.Domain.Entities;
using GestorTarea.Application.DTOs;


namespace GestorTarea.Application.Services
{
    public class GestorTareasService
    {
        private readonly ITareaRepositorio _repositorio;

        public GestorTareasService(ITareaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public List<TareaDTO> ObtenerTareas()
        {
            var tareas = _repositorio.ObtenerTodas();
            return tareas.Select(tarea => new TareaDTO
            {
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaLimite = tarea.DiaVencimiento,
                Estado = tarea.Estado.ToString(),
                UsuarioID = tarea.UsuarioID,
                TipoTarea = tarea.GetType().Name
            }).ToList();
        }
        public bool AgregarTareaDesdeDTO(TareaDTO dto)
        {
            Tarea nuevaTarea;

            switch (dto.TipoTarea.ToLower())
            {
                case "simple":
                    nuevaTarea = new TareaSimple(dto.Titulo, dto.Descripcion, dto.FechaLimite, dto.UsuarioID);
                    break;
                case "urgente":
                    nuevaTarea = new TareaUrgente(dto.Titulo, dto.Descripcion, dto.FechaLimite, dto.UsuarioID);
                    break;
                case "recurrente":
                    // Suponiendo que le pasas un intervalo por defecto si el DTO no lo tiene
                    nuevaTarea = new TareaRecurrente(dto.Titulo, dto.Descripcion, dto.FechaLimite,
                        dto.UsuarioID, new List<string>{ "L", "Ma", "Mi", "J", "V"} );
                    break;
                // Añade aquí TareaPomodoro y TareaConTarea según los constructores que tengas
                default:
                    throw new ArgumentException("Tipo de tarea no válido");
            }

            // falta mapear el estado y otros campos

            _repositorio.Agregar(nuevaTarea);
            return true;
        }
    }
}
