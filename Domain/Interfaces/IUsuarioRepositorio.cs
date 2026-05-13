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
    }
}
