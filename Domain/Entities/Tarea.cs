using System;
using static System.Net.WebRequestMethods;

namespace GestorTarea.Domain.Entities
{
    public enum EstadoTarea
    {
        Pendiente,
        EnProgreso,
        Completada,
        Cancelada,
        Vencida
    }
    public abstract class Tarea
    {
        public string titulo { get; set; }
        public int ID { get; init; }
        public int UsuarioID { get; set; }
        public string Description { get; set; }

        public DateTime InitDay;
        public DateTime Endday { get; set; }
        private EstadoTarea _estado;
        private string _motivoCancelacion = "";
        
        public EstadoTarea Estado
        {
            get
            {
                
                if (DateTime.Now > Endday)
                {
                    return EstadoTarea.Vencida;
                }
                else
                {
                    return _estado;
                }
            }
            set
            {
                _estado = value;
            }
        }

        protected Tarea(string titulo, string description,
          DateTime endDay, int usuarioID)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("El título no puede estar vacío o contener solo espacios en blanco."
                , nameof(titulo));
            }
            if (endDay < DateTime.Now.AddMinutes(-1))
            {
                throw new ArgumentException(
                    "La fecha limite no puede ser anterior a hoy");
            }
            else
            {
                this.titulo = titulo.Trim(); // Elimina espacios en blanco al inicio y al final del título
                this.Description = description?.Trim() ?? string.Empty; // Si description es null, se asigna una cadena vacía
                this.InitDay = DateTime.Now;
                this.Endday = endDay;
                this.ID = CalcularID(titulo);
                this._estado = EstadoTarea.Pendiente;
                this.UsuarioID = usuarioID;
                
            }

        }
        public static int CalcularID(string titulo)
        {
            return titulo.GetHashCode();
        }

        public int DiasRestantes => (Endday - DateTime.Now).Days;
        public bool EstaVencida()
        {
            if (_estado != EstadoTarea.Completada && DateTime.Now > Endday && _estado != EstadoTarea.Cancelada)
            {
                return true;
            }
            return false;
        }

        public void IniciarTarea() => Estado = EstadoTarea.EnProgreso;
        public void ReabrirTarea() => Estado = EstadoTarea.Pendiente;

        public void ActualizarDatos(string descrip, string tituloNuevo)
        {
            this.Description = descrip;
            this.titulo = tituloNuevo;
        }
        public bool CompletarTarea()
        {
            if (_estado == EstadoTarea.Cancelada)
            {
                return false;
            }

            _estado = EstadoTarea.Completada;
            return true;
        }

        public bool CancelarTarea(string motivo)
        {
            if (_estado == EstadoTarea.Cancelada) return false;

            _estado = EstadoTarea.Cancelada;
            _motivoCancelacion = motivo ?? "Sin motivo especificado";
            return true;
        }

        public override string ToString()
        {
            return $"ID: {ID}" +
            $", Título: {titulo}" +
            $", Descripción: {Description}" +
            $", Fecha de Inicio: {InitDay.ToShortDateString()}" +
            $", Fecha de Fin: {Endday.ToShortDateString()}" +
            $", Estado: {Estado}";
        }
        public virtual void ObtenerResumen()
        {
            Console.WriteLine($"ID: {ID} - Título: {titulo} - Estado: {Estado}");
        }
    }
}
