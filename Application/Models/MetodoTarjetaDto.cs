using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Application.Models
{
    public class MetodoTarjetaDto
    {
        // null OK | "" NO | solo números | 16 dígitos
        [RegularExpression(@"^\d{16}$", ErrorMessage = "El número de la tarjeta debe tener 16 dígitos numéricos")]
        public string? NumeroTarjeta { get; set; }
        public string? NombreTitular { get; set; }

        // null OK | "" NO | formato MM/AA
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "La fecha debe tener el formato MM/AA")]
        public string? FechaExpiracion { get; set; }

        // null OK | "" NO | 3 dígitos
        [RegularExpression(@"^\d{3}$", ErrorMessage = "El código de seguridad debe tener 3 dígitos numéricos")]
        public string? CodigoSeguridad { get; set; }
    }
}
