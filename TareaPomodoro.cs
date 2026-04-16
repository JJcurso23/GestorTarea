using System;

namespace Tareas
{
    public class TareaPomodoro: Tarea
    {
        public DateTime TiempoFinal { get; set; }
        public DateTime tiempoInit { get; set; }
        private int _sesiones;

        public TareaPomodoro(string titulo, string description, 
            DateTime endDay, int usuarioID)
            : base(titulo, description, endDay, usuarioID)
        { }

        public void IniciarTemporizador()
        {
            tiempoInit = DateTime.Now;
            _sesiones++;
        }

        public int ContarSesiones() => _sesiones;

        public override void ObtenerResumen()
        {
            Console.WriteLine($"Pomodoro {titulo} - Sesiones {_sesiones}");
        }
    }
}

