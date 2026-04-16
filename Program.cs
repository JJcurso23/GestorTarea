using System;
using Tareas;

class Program
{
    static void Main(string[] args)
    {
        GestorTarea gestor = new GestorTarea();
        string ruta = "tareas.json";

        gestor.Cargar(ruta);

        TareaSimple tarea1 = new TareaSimple(
            "Estudiar C#",
            "Repasar POO",
            DateTime.Today.AddDays(3)
        );

        gestor.AgregarTarea(tarea1);

        foreach (var t in gestor.ObtenerTareas())
        {
            t.ObtenerResumen();
        }

        gestor.Guardar(ruta);
    }
}