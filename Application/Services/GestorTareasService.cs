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
        private readonly ILogger<GestorTareasService> _logger;

        public GestorTareasService(ITareaRepositorio repositorio, ILogger<GestorTareasService> logger)
        {
            _repositorio = repositorio;
            _logger = logger;
        }

        public PaginadoResponseDto <TareaResponseDTO> ObtenerPaginadas(int pagina, int porPagina, string? estado, int usuarioId, bool esAdmin)
        {
            var paginado = esAdmin
                ? _repositorio.ObtenerPaginadas(pagina, porPagina, estado)
                : _repositorio.ObtenerPaginadasPorUsuario(pagina, porPagina, estado, usuarioId);

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

        public TareaResponseDTO? ObtenerTareaPorId(int id, int usuarioId, bool esAdmin)
        {
            var tarea = _repositorio.ObtenerPorId(id);
            if (tarea == null) return null;
            if (!esAdmin && tarea.UsuarioID != usuarioId) return null;

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

        public enum ResultadoOperacion { Ok, NoEncontrada, Prohibida }

        public ResultadoOperacion EliminarTarea(int id, int usuarioId, bool esAdmin)
        {
            var tarea = _repositorio.ObtenerPorId(id);
            if (tarea == null) return ResultadoOperacion.NoEncontrada;
            if (!esAdmin && tarea.UsuarioID != usuarioId) return ResultadoOperacion.Prohibida;

            _repositorio.Eliminar(tarea);
            return ResultadoOperacion.Ok;
        }

        public ResultadoOperacion CompletarTarea(int id, int usuarioId, bool esAdmin)
        {
            var tarea = _repositorio.ObtenerPorId(id);
            if (tarea == null) return ResultadoOperacion.NoEncontrada;
            if (!esAdmin && tarea.UsuarioID != usuarioId) return ResultadoOperacion.Prohibida;

            if (!tarea.CompletarTarea())
                return ResultadoOperacion.Prohibida;

            _repositorio.Actualizar(tarea);
            return ResultadoOperacion.Ok;
        }

        public ResultadoOperacion ReabrirTarea(int id, int usuarioId, bool esAdmin)
        {
            var tarea = _repositorio.ObtenerPorId(id);
            if (tarea == null) return ResultadoOperacion.NoEncontrada;
            if (!esAdmin && tarea.UsuarioID != usuarioId) return ResultadoOperacion.Prohibida;

            tarea.ReabrirTarea();
            _repositorio.Actualizar(tarea);
            return ResultadoOperacion.Ok;
        }

        public ResultadoOperacion CancelarTarea(int id, string? motivo, int usuarioId, bool esAdmin)
        {
            var tarea = _repositorio.ObtenerPorId(id);
            if (tarea == null) return ResultadoOperacion.NoEncontrada;
            if (!esAdmin && tarea.UsuarioID != usuarioId) return ResultadoOperacion.Prohibida;

            if (!tarea.CancelarTarea(motivo ?? "Sin motivo"))
                return ResultadoOperacion.Prohibida;

            _repositorio.Actualizar(tarea);
            return ResultadoOperacion.Ok;
        }

        public ResultadoOperacion ActualizarTarea(int id, TareaDTO dto, int usuarioId, bool esAdmin)
        {
            var tareaExistente = _repositorio.ObtenerPorId(id);
            if (tareaExistente == null) return ResultadoOperacion.NoEncontrada;
            if (!esAdmin && tareaExistente.UsuarioID != usuarioId) return ResultadoOperacion.Prohibida;

            tareaExistente.ActualizarDatosCompletos(
                dto.Titulo,
                dto.Descripcion ?? "",
                dto.FechaLimite);

            _repositorio.Actualizar(tareaExistente);
            return ResultadoOperacion.Ok;
        }
        public bool AgregarTareaDesdeDTO(TareaDTO dto)
        {
            _logger.LogInformation(
                "Creando tarea con titulo: {Titulo} para usuario {UsuarioId}",
                dto.Titulo, dto.UsuarioID);

            if (string.IsNullOrWhiteSpace(dto.Titulo))
            {
                _logger.LogWarning("Intento de crear tarea con titulo vacio");
                throw new ArgumentException("El titulo no puede estar vacio");
            }
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

            _logger.LogInformation($"Tarea creada con Id: {nuevaTarea.ID}");
            return true;
        }
    }
}
