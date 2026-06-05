using GestorTarea.Domain.Entities;
using GestorTarea.Domain.Interfaces;
using GestorTarea.Infrastructure.Data;

namespace GestorTarea.Infrastructure.Repositories
{
    public class UsuarioRepositorio: IUsuarioRepositorio
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
        public Usuario? ObtenerPorEmail(string email)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email);
        }

        public Dictionary<int, string> ObtenerNombresPorIds(IEnumerable<int> ids)
        {
            var idsUnicos = ids.Distinct().ToList();
            return _context.Usuarios
                .Where(u => idsUnicos.Contains(u.Id))
                .ToDictionary(u => u.Id, u => u.Nombre);
        }
    }
}
