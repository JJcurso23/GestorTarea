namespace GestorTarea.Application.DTOs
{
    public class UsuarioDTO
    {
        public required string Nombre { get; set; }
        public required string Email { get; set; }
        public int Edad { get; set; }
        public bool Activo { get; set; } = true;
    }
}
