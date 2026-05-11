using GestorTarea.Domain.Entities;

namespace GestorTarea.Application.DTOs
{
    public class PaginadoResponseDto<T>
    {
        public required List<T> Items { get; set; }
        public int NumerodeTarea { get; set; }
        public int PaginaActual { get; set; }
        public int PaginaTotal { get; set; }

    }
}
