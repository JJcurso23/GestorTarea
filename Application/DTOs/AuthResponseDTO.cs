namespace GestorTarea.Application.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expira { get; set; }
    }
}
