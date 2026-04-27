using GestorTarea.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorTarea.Domain.Interfaces
{
    public interface ITareaRepositorio
    {
        public List<TareaDTO> ObtenerTodas();
        public void Guardar(List<TareaDTO> tareas);
    }
}
