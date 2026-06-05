using System;

namespace GestorTarea.Domain.Entities
{
    public class TareaPomodoro : Tarea
    {
        /// <summary>Duración de cada sesión de concentración en minutos.</summary>
        public int DuracionMinutos { get; set; } = 25;

        /// <summary>Inicio de la última sesión iniciada (null si nunca se ha iniciado).</summary>
        public DateTime? TiempoInit { get; set; }

        /// <summary>Fin teórico de la última sesión (TiempoInit + DuracionMinutos).</summary>
        public DateTime? TiempoFinal { get; set; }

        /// <summary>Número de sesiones completadas.</summary>
        public int Sesiones { get; set; }

        protected TareaPomodoro() : base() { }

        public TareaPomodoro(string titulo, string description,
            DateTime endDay, int usuarioID, int duracionMinutos = 25)
            : base(titulo, description, endDay, usuarioID)
        {
            if (duracionMinutos < 1) duracionMinutos = 25;
            DuracionMinutos = duracionMinutos;
        }

        /// <summary>Marca el inicio de una nueva sesión de concentración.</summary>
        public void IniciarTemporizador()
        {
            TiempoInit = DateTime.UtcNow;
            TiempoFinal = TiempoInit.Value.AddMinutes(DuracionMinutos);
        }

        /// <summary>Registra que una sesión se completó.</summary>
        public void RegistrarSesionCompletada()
        {
            Sesiones++;
            TiempoInit = null;
            TiempoFinal = null;
        }

        public override void ObtenerResumen()
        {
            Console.WriteLine($"Pomodoro {Titulo} - {DuracionMinutos} min - Sesiones {Sesiones}");
        }
    }
}
