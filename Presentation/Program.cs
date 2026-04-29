using System.Text.Json;
using GestorTarea.Domain.Entities;
using SistemaTareas.GestorTareas.Application.Services;

class Program
{
    static void Main(string[] args)
    {
        GestorTareasService gestor = new GestorTareasService();
        string ruta = "tareas.json";
        string rutaUsuario = "usuarios.json";
        string jsonUsuarios = File.ReadAllText(rutaUsuario);

        List<Usuario> usuarios =
            JsonSerializer.Deserialize<List<Usuario>>(jsonUsuarios);

        gestor.Cargar(ruta);

        Console.WriteLine("Resumen actual del gestos: ");
        TareaSimple nuevaTarea = new TareaSimple(
        "Nueva Tarea desde código",
        "Esta tarea se guardará en el JSON",
        DateTime.Now.AddDays(5),
        1 // ID de usuario
    );

        if (gestor.AgregarTarea(nuevaTarea))
        {
            Console.WriteLine("Tarea agregada al gestor.");
        }

        foreach (var t in gestor.ObtenerTareas())
        {
            t.ObtenerResumen();
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
                Console.WriteLine($"titulo de libros por usuario {grupo.Key}: {tarea.Titulo}");
            }
        }
    }
}