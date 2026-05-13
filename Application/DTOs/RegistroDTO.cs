using System.ComponentModel.DataAnnotations;

namespace GestorTarea.Application.DTOs
{
    public class RegistroDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;
        [Required, EmailAddress(ErrorMessage = "Formato de email no valido")]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8, ErrorMessage = "Minimo 8 caracteres")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "edad es obligatorio")]
        public int Edad { get; set; } = 0;
    }
}
