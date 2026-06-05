using GestorTarea.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using GestorTarea.Infrastructure.Repositories;

namespace GestorTarea.Domain.Interfaces
{
    public interface IUsuarioRepositorio
    {
        public Usuario? ObtenerPorId(int id);

        public Usuario? ObtenerPorEmail(string email);
        public void Agregar(Usuario usuario);

        /// <summary>Devuelve un diccionario {usuarioId -> nombre} para los ids pedidos.</summary>
        public Dictionary<int, string> ObtenerNombresPorIds(IEnumerable<int> ids);
    }
}
