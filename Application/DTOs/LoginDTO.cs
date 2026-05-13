using System.ComponentModel.DataAnnotations;

namespace GestorTarea.Application.DTOs
{
    public class LoginDTO
    {
        
        [Required, EmailAddress(ErrorMessage = "Formato de email no valido")]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8, ErrorMessage ="Minimo 8 caracteres")]
        public string Password { get; set; } = string.Empty;
    }
}
