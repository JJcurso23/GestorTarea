using System;
using GestorTarea.Domain.Enums;
using static System.Net.WebRequestMethods;

namespace GestorTarea.Domain.Entities
{
   
    public abstract class Tarea
    {
        
        public string Titulo { get; protected set; }
        public int ID { get; init; }
        public int UsuarioID { get; protected set; }
        public string Descripcion { get; protected set; }

        public DateTime Diainicio { get; protected set; }
        public DateTime DiaVencimiento { get; protected set; }
        public string Responsable { get; protected set; }
        private EstadoTarea _estado;
        private string _motivoCancelacion = "";
        
        
        public EstadoTarea Estado
        {
            get
            {
                // Los estados terminales mandan sobre el cálculo por fecha.
                if (_estado == EstadoTarea.Completada
                 || _estado == EstadoTarea.Cancelada)
                {
                    return _estado;
                }

                if (DateTime.Now > DiaVencimiento)
                {
                    return EstadoTarea.Vencida;
                }

                return _estado;
            }
            set
            {
                _estado = value;
            }
        }
        protected Tarea() { }

        protected Tarea(string titulo, string descripcion,
          DateTime diaVencimiento, int usuarioID)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("El título no puede estar vacío o contener solo espacios en blanco."
                , nameof(titulo));
            }
            if (diaVencimiento < DateTime.Now.AddMinutes(-1))
            {
                throw new ArgumentException(
                    "La fecha limite no puede ser anterior a hoy");
            }
            else
            {
                this.Titulo = titulo.Trim(); // Elimina espacios en blanco al inicio y al final del título
                this.Descripcion = descripcion?.Trim() ?? string.Empty; // Si description es null, se asigna una cadena vacía
                this.Diainicio = DateTime.Now;
                this.DiaVencimiento = diaVencimiento;
                this._estado = EstadoTarea.Pendiente;
                this.UsuarioID = usuarioID;
                this.Responsable = "Sin asignar";

            }

        }
        

        public int DiasRestantes => (DiaVencimiento - DateTime.Now).Days;
        public bool EstaVencida()
        {
            if (_estado != EstadoTarea.Completada 
                && DateTime.Now > DiaVencimiento 
                && _estado != EstadoTarea.Cancelada)
            {
                return true;
            }
            return false;
        }

        public void IniciarTarea() => Estado = EstadoTarea.EnProgreso;

        public void ReabrirTarea()
        {
            _estado = EstadoTarea.Pendiente;
            // Si la fecha ya pasó, empujamos el vencimiento 7 días para que la tarea
            // vuelva realmente a estar activa y no recaiga inmediatamente en Vencida.
            if (DateTime.Now > DiaVencimiento)
            {
                DiaVencimiento = DateTime.Now.AddDays(7);
            }
        }

        public void ActualizarDatosCompletos(string titulo,
            string descrip, DateTime nuevaFecha)
        {
            if (string.IsNullOrWhiteSpace(titulo)) 
                throw new ArgumentException("El titulo es obligatorio");

            this.Titulo = titulo;
            this.Descripcion = descrip.Trim();
            this.DiaVencimiento = nuevaFecha;

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
            $", Título: {Titulo}" +
            $", Descripción: {Descripcion}" +
            $", Fecha de Inicio: {Diainicio.ToShortDateString()}" +
            $", Fecha de Fin: {DiaVencimiento.ToShortDateString()}" +
            $", Estado: {Estado}";
        }
        public virtual void ObtenerResumen()
        {
            Console.WriteLine($"ID: {ID} - Título: {Titulo} - Estado: {Estado}");
        }
    }
}
