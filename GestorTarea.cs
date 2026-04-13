using System;

public class GestorTarea
{
    public List<Tarea> Tareas { get; set; }
    public GestorTarea()
    {
        Tareas = new List<Tarea>();
    }
    public void AgregarTarea(Tarea tarea)
    {
        Tareas.Add(tarea);
    }
    public void EliminarTarea(Tarea tarea)
    {
        Tareas.Remove(tarea);
    }
    public List<Tarea> ObtenerTareas()
    {
        return Tareas;
    }

}
