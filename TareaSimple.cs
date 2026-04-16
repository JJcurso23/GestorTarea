using System;


namespace Tareas
{
    public class TareaSimple : Tarea
    {
        public TareaSimple(string titulo, string description, DateTime endDay, int usuarioID)
            : base(titulo, description, endDay, usuarioID)
        {
        }
        public override void ObtenerResumen()
        {
            base.ObtenerResumen();
            
        }
    }
    
}

