using GestorTarea.Domain.Entities;
using GestorTarea.Infrastructure.Data;

namespace GestorTarea.Infrastructure.Repositories
{
    public class UsuarioRepositorio
    {
        private readonly GestorTareasContext _context;

        public UsuarioRepositorio(GestorTareasContext context)
        {
            _context = context;
        }
        public Usuario? ObtenerPorId(int id) => _context.Usuarios.Find(id);

        public void Agregar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }
    }
}
