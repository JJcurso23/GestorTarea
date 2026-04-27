using GestorTarea.Application.DTOs;
using GestorTarea.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace GestorTarea.Infrastructure.Repositories
{
    public class TareaRepositorio: ITareaRepositorio
    {
        private readonly string _ruta = "tareas.json";

        public List<TareaDTo> ObtenerTodas()
        {
            if (!File.Exists(_ruta)) return new List<TareaDTo>();
            string json = File.ReadAllText(_ruta);
            return JsonSerializer.Deserialize<List<TareaDTo>>(json);
        }

        public void Guardar(List<TareaDTo> tareas)
        {
            string json = JsonSerializer.Serialize(tareas);
            File.WriteAllText(_ruta, json);
        }
    }
}
