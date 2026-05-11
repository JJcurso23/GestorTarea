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

        public PaginadoResponseDto <TareaResponseDTO> ObtenerPaginadas(int pagina, int porPagina, string? estado)
        {
            var paginado = _repositorio.ObtenerPaginadas(pagina, porPagina, estado);

            var listaMapeada = paginado.Item1.Select(tarea => new TareaResponseDTO
            {
                Id = tarea.ID,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaLimite = tarea.DiaVencimiento,
                Estado = tarea.Estado.ToString(),
                UsuarioID = tarea.UsuarioID,
                TipoTarea = tarea.GetType().Name
            }).ToList();

            return new PaginadoResponseDto<TareaResponseDTO>
            {
                Items = listaMapeada, // Aquí metemos la lista que acabamos de transformar
                NumerodeTarea = paginado.Item2, // El conteo que viene del repositorio
                PaginaActual = pagina,
                PaginaTotal = (int)Math.Ceiling(paginado.Item2 / (double)porPagina) // La fórmula matemática

            };
        }

        public List<TareaResponseDTO> ObtenerTareas()
        {
            var tareas = _repositorio.ObtenerTodas();
            return tareas.Select(tarea => new TareaResponseDTO
            {
                Id = tarea.ID,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaLimite = tarea.DiaVencimiento,
                Estado = tarea.Estado.ToString(),
                UsuarioID = tarea.UsuarioID,
                TipoTarea = tarea.GetType().Name
            }).ToList();
        }

        public TareaResponseDTO? ObtenerTareaPorId(int id)
        {
            var tarea = _repositorio.ObtenerPorId(id);
            if (tarea == null) return null;

            return new TareaResponseDTO
            {
                Id = tarea.ID,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaLimite = tarea.DiaVencimiento,
                Estado = tarea.Estado.ToString(),
                UsuarioID = tarea.UsuarioID,
                TipoTarea = tarea.GetType().Name
            };

        }

        public bool EliminarTarea(int id)
        {
            var tarea = _repositorio.ObtenerPorId(id);
            if (tarea == null) return false;

            _repositorio.Eliminar(tarea);
            return true;
        }

        public bool ActualizarTarea(int id, TareaDTO dto)
        {
            var tareaExistente = _repositorio.ObtenerPorId(id);
            if (tareaExistente == null) return false;

            tareaExistente.ActualizarDatosCompletos(
                dto.Titulo,
                dto.Descripcion ?? "",
                dto.FechaLimite);

            _repositorio.Actualizar(tareaExistente);
            return true;
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

            

            _repositorio.Agregar(nuevaTarea);
            return true;
        }
    }
}
