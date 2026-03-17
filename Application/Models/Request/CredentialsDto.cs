using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class CredentialsDto
    {
        [Required(ErrorMessage = "El teléfono es requerido")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "El teléfono debe tener entre 7 y 15 caracteres")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El teléfono solo puede contener números")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; } = string.Empty;
    }
}