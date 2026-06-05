using GestorTarea.Application.DTOs;
using GestorTarea.Domain.Interfaces;
using GestorTarea.Infrastructure.Data;
using GestorTarea.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace GestorTarea.Infrastructure.Repositories
{
    public class TareaRepositorio: ITareaRepositorio
    {
        private readonly GestorTareasContext _context;

        public TareaRepositorio(GestorTareasContext context)
        {
            _context = context;
        }
        public List<Tarea> ObtenerTodas()
        {
            return _context.Tareas.ToList();
        }
        public void Agregar(Tarea tarea)
        {
            _context.Tareas.Add(tarea);
            _context.SaveChanges();
        }
        public Tarea? ObtenerPorId(int id)
        {
            return _context.Tareas.Find(id);
        }
        public void Actualizar(Tarea tarea)
        {
            _context.Tareas.Update(tarea);
            _context.SaveChanges();
        }
        public void Eliminar(Tarea tarea)
        {
            _context.Tareas.Remove(tarea);
            _context.SaveChanges();
        }

        public (List<Tarea>, int) ObtenerPaginadas(int pagina, int porPagina, string? estado)
        {
            var consulta = _context.Tareas.AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(t => t.Estado.ToString() == estado);
            }

            int totalRegistros = consulta.Count();

            var resultadoFinal = consulta
                .OrderBy(t => t.ID)
                .Skip((pagina -1)*porPagina)
                .Take(porPagina)
                .ToList();

            return (resultadoFinal, totalRegistros);

        }

        public (List<Tarea>, int) ObtenerPaginadasPorUsuario(int pagina, int porPagina, string? estado, int usuarioId)
        {
            var consulta = _context.Tareas.Where(t => t.UsuarioID == usuarioId);

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(t => t.Estado.ToString() == estado);
            }

            int totalRegistros = consulta.Count();

            var resultadoFinal = consulta
                .OrderBy(t => t.ID)
                .Skip((pagina - 1) * porPagina)
                .Take(porPagina)
                .ToList();

            return (resultadoFinal, totalRegistros);
        }
    }
}
