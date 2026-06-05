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
        private readonly IUsuarioRepositorio _usuarios;
        private readonly ILogger<GestorTareasService> _logger;

        public GestorTareasService(
            ITareaRepositorio repositorio,
            IUsuarioRepositorio usuarios,
            ILogger<GestorTareasService> logger)
        {
            _repositorio = repositorio;
            _usuarios = usuarios;
            _logger = logger;
        }

        /// <summary>Mapea una Tarea a su DTO de respuesta, incluyendo los campos especificos por tipo.</summary>
        /// <param name="nombresPorUsuario">Diccionario opcional para enriquecer el DTO con UsuarioNombre.</param>
        private static TareaResponseDTO MapearADto(Tarea tarea, IReadOnlyDictionary<int, string>? nombresPorUsuario = null)
        {
            var dto = new TareaResponseDTO
            {
                Id = tarea.ID,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaLimite = tarea.DiaVencimiento,
                Estado = tarea.Estado.ToString(),
                UsuarioID = tarea.UsuarioID,
                TipoTarea = tarea.GetType().Name
            };

            if (nombresPorUsuario != null && nombresPorUsuario.TryGetValue(tarea.UsuarioID, out var nombre))
            {
                dto.UsuarioNombre = nombre;
            }

            switch (tarea)
            {
                case TareaPomodoro p:
                    dto.DuracionMinutos = p.DuracionMinutos;
                    dto.Sesiones = p.Sesiones;
                    dto.TiempoInit = p.TiempoInit;
                    dto.TiempoFinal = p.TiempoFinal;
                    break;
                case TareaRecurrente r:
                    dto.IntervaloDias = r.IntervaloDias;
                    break;
            }

            return dto;
        }

        public PaginadoResponseDto<TareaResponseDTO> ObtenerPaginadas(int pagina, int porPagina, string? estado, int usuarioId, bool esAdmin)
        {
            var paginado = esAdmin
                ? _repositorio.ObtenerPaginadas(pagina, porPagina, estado)
                : _repositorio.ObtenerPaginadasPorUsuario(pagina, porPagina, estado, usuarioId);

            // Cargamos los nombres de los usuarios involucrados en una sola consulta.
            var idsUsuarios = paginado.Item1.Select(t => t.UsuarioID).Distinct().ToList();
            var nombres = idsUsuarios.Count > 0
                ? _usuarios.ObtenerNombresPorIds(idsUsuarios)
                : new Dictionary<int, string>();

            var listaMapeada = paginado.Item1.Select(t => MapearADto(t, nombres)).ToList();

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
            return _repositorio.ObtenerTodas().Select(t => MapearADto(t)).ToList();
        }

        public TareaResponseDTO? ObtenerTareaPorId(int id, int usuarioId, bool esAdmin)
        {
            var tarea = _repositorio.ObtenerPorId(id);
            if (tarea == null) return null;
            if (!esAdmin && tarea.UsuarioID != usuarioId) return null;

            var nombres = _usuarios.ObtenerNombresPorIds(new[] { tarea.UsuarioID });
            return MapearADto(tarea, nombres);
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

            // Si la tarea completada es recurrente, generar automaticamente la siguiente.
            if (tarea is TareaRecurrente recurrente)
            {
                var siguiente = recurrente.GenerarSiguiente();
                _repositorio.Agregar(siguiente);
                _logger.LogInformation(
                    "Tarea recurrente {Id} completada -> generada siguiente con Id {NextId}",
                    recurrente.ID, siguiente.ID);
            }

            return ResultadoOperacion.Ok;
        }

        public ResultadoOperacion IniciarPomodoro(int id, int usuarioId, bool esAdmin)
        {
            var tarea = _repositorio.ObtenerPorId(id);
            if (tarea == null) return ResultadoOperacion.NoEncontrada;
            if (!esAdmin && tarea.UsuarioID != usuarioId) return ResultadoOperacion.Prohibida;

            if (tarea is not TareaPomodoro pomodoro)
                return ResultadoOperacion.Prohibida;

            pomodoro.IniciarTemporizador();
            _repositorio.Actualizar(pomodoro);
            return ResultadoOperacion.Ok;
        }

        public ResultadoOperacion CompletarSesionPomodoro(int id, int usuarioId, bool esAdmin)
        {
            var tarea = _repositorio.ObtenerPorId(id);
            if (tarea == null) return ResultadoOperacion.NoEncontrada;
            if (!esAdmin && tarea.UsuarioID != usuarioId) return ResultadoOperacion.Prohibida;

            if (tarea is not TareaPomodoro pomodoro)
                return ResultadoOperacion.Prohibida;

            pomodoro.RegistrarSesionCompletada();
            _repositorio.Actualizar(pomodoro);
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
                    nuevaTarea = new TareaSimple(dto.Titulo, dto.Descripcion ?? "", dto.FechaLimite, dto.UsuarioID);
                    break;
                case "urgente":
                    nuevaTarea = new TareaUrgente(dto.Titulo, dto.Descripcion ?? "", dto.FechaLimite, dto.UsuarioID);
                    break;
                case "recurrente":
                    nuevaTarea = new TareaRecurrente(
                        dto.Titulo,
                        dto.Descripcion ?? "",
                        dto.FechaLimite,
                        dto.UsuarioID,
                        dto.IntervaloDias ?? 1);
                    break;
                case "pomodoro":
                    nuevaTarea = new TareaPomodoro(
                        dto.Titulo,
                        dto.Descripcion ?? "",
                        dto.FechaLimite,
                        dto.UsuarioID,
                        dto.DuracionMinutos ?? 25);
                    break;
                default:
                    throw new ArgumentException("Tipo de tarea no válido");
            }

            

            _repositorio.Agregar(nuevaTarea);

            _logger.LogInformation($"Tarea creada con Id: {nuevaTarea.ID}");
            return true;
        }
    }
}
