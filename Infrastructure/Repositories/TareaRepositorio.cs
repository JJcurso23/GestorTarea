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
    }
}
