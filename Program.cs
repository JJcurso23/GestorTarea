using System;
using Tareas;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {  
        GestorTarea gestor = new GestorTarea();
        string ruta = "tareas.json";
        string rutaUsuario = "usuarios.json";
        string jsonUsuarios = File.ReadAllText(rutaUsuario);

        List<Usuario> usuarios =
            JsonSerializer.Deserialize<List<Usuario>>(jsonUsuarios);

        gestor.Cargar(ruta);

        TareaSimple tarea1 = new TareaSimple(
            "Estudiar C#",
            "Repasar POO",
            DateTime.Today.AddDays(3),
            usuarios[1].Id

        );

        gestor.AgregarTarea(tarea1);

        foreach (var t in gestor.ObtenerTareas())
        {
            t.ObtenerResumen();
            t.ToString();
        }

        gestor.Guardar(ruta);

        List<Tarea> tareas = gestor.ObtenerTareas();

        var tareasPorUsuario =
        tareas.GroupBy(t => t.UsuarioID);

        foreach (var grupo in tareasPorUsuario)
        {
            Console.WriteLine($"Usuario {grupo.Key}");

            foreach (var tarea in grupo)
            {
                Console.WriteLine($"titulo de libros por usuario {grupo.Key}: {tarea.titulo}");
            }
        }
    }
}