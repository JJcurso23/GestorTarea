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

        public List<TareaDTO> ObtenerTodas()
        {
            if (!File.Exists(_ruta)) return new List<TareaDTO>();
            string json = File.ReadAllText(_ruta);
            return JsonSerializer.Deserialize<List<TareaDTO>>(json);
        }

        public void Guardar(List<TareaDTO> tareas)
        {
            string json = JsonSerializer.Serialize(tareas);
            File.WriteAllText(_ruta, json);
        }
    }
}
