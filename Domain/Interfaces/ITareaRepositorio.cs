using GestorTarea.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorTarea.Domain.Interfaces
{
    public interface ITareaRepositorio
    {
        public List<TareaDTo> ObtenerTodas();
        public void Guardar(List<TareaDTo> tareas);
    }
}
