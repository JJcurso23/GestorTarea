using System;


namespace Tareas
{
    public class TareaSimple : Tarea
    {
        public TareaSimple(string titulo, string description, DateTime endDay)
            : base(titulo, description, endDay)
        {
        }
        public override void ObtenerResumen()
        {
            base.ObtenerResumen();
            
        }
    }
    
}

