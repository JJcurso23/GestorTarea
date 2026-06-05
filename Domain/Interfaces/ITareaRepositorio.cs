using GestorTarea.Application.DTOs;
using GestorTarea.Domain.Entities;
using GestorTarea.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorTarea.Domain.Interfaces
{
    public interface ITareaRepositorio
    {
        public List<Tarea> ObtenerTodas();
        Tarea? ObtenerPorId(int id);
        public void Agregar(Tarea tarea);
        public void Actualizar(Tarea tarea);
        public void Eliminar(Tarea tarea);

        public (List<Tarea>, int) ObtenerPaginadas(int pagina, int porPagina, string? estado);
        public (List<Tarea>, int) ObtenerPaginadasPorUsuario(int pagina, int porPagina, string? estado, int usuarioId);
    }
}
