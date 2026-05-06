using System;
using System.Text.Json;
using System.IO;
using GestorTarea.Domain.Entities;
using GestorTarea.Application.DTOs;

namespace SistemaTareas.GestorTareas.Application.Services
{
    public class GestorTareasService
    {
        public Dictionary<int, Tarea> Tareas { get; set; }
        public GestorTareasService()
        {
            Tareas = new Dictionary<int, Tarea>();
        }
        public bool AgregarTarea(Tarea tarea)
        {
            if (!Tareas.ContainsKey(tarea.ID)) {
                Tareas.Add(tarea.ID, tarea);
                return true;
            }
            return false;
        }

        public void EliminarTarea(int id)
        {
            Tareas.Remove(id);
        }
        public List<Tarea> ObtenerTareas()
        {
            return Tareas.Values.ToList();
        }

      
        public void Guardar(string ruta)
        {
           
            var listaDto = new List<TareaDTO>();

            foreach (var tarea in Tareas.Values)
            {
                listaDto.Add(new TareaDTO
                {
                   
                    Titulo = tarea.Titulo,
                    Descripcion = tarea.Descripcion,
                    FechaLimite = tarea.DiaVencimiento,
                    Estado = tarea.Estado.ToString(),
                    UsuarioID = tarea.UsuarioID,
                    TipoTarea = tarea is TareaConTarea ? "Dependiente" : "Simple"
                });
            }

            
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            try
            {
                string json = JsonSerializer.Serialize(listaDto, opciones);
                File.WriteAllText(ruta, json);
                Console.WriteLine("¡Datos guardados exitosamente en el JSON!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar en el JSON: {ex.Message}");
            }
        }

        public void Cargar(string ruta)
        {
            if (!File.Exists(ruta))
            {
                this.Tareas = new Dictionary<int, Tarea>();
                Console.WriteLine("No existe un archivo de dato previo.");
                return;
            }

            try
            {
                string json = File.ReadAllText(ruta);
                var opciones = new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true };
                var listaDto = JsonSerializer.Deserialize<List<TareaDTO>>(json);
                Tareas = new Dictionary<int, Tarea>();
                foreach (var dto in listaDto)
                {
                    TareaSimple tarea = new TareaSimple(
                        dto.Titulo,
                        dto.Descripcion,
                        dto.FechaLimite,
                        dto.UsuarioID);

                    if (!Tareas.ContainsKey(tarea.ID))
                    {
                        Tareas.Add(tarea.ID, tarea);
                        // Imprimimos la confirmación de carga
                        Console.WriteLine($"[Cargada] ID: {tarea.ID} | Título: {tarea.Titulo}");
                    }
                }
                Console.WriteLine("--- Carga completa con exito ---\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error critico al cargar datos: {ex.Message}");
            }
        }
    }
}


