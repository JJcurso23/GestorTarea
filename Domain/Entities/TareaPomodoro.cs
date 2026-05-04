using System;

namespace GestorTarea.Domain.Entities
{
    public class TareaPomodoro: Tarea
    {
        public DateTime TiempoFinal { get; set; }
        public DateTime TiempoInit { get; set; }
        private int _sesiones;

        protected TareaPomodoro() : base() { }
        public TareaPomodoro(string titulo, string description, 
            DateTime endDay, int usuarioID)
            : base(titulo, description, endDay, usuarioID)
        { }

        public void IniciarTemporizador()
        {
            TiempoInit = DateTime.Now;
            _sesiones++;
        }

        public int ContarSesiones() => _sesiones;

        public override void ObtenerResumen()
        {
            Console.WriteLine($"Pomodoro {Titulo} - Sesiones {_sesiones}");
        }
    }
}

