using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Application.Models.Request
{
    public class CreationMetodoPagoDto
    {
        [Required(ErrorMessage = "El Id del usuario es requerido")]
        public int UsuarioId { get; set; }
        [Required(ErrorMessage = "El tipo de metodo de pago es requerido")]
        public TipoMetodoPago TipoMetodo { get; set; }
        [Required(ErrorMessage = "Los detalles del metodo de pago son requeridos")]

        public MetodoTarjetaDto? Tarjeta { get; set; }
        public string Detalles { get; set; } = string.Empty;
    }

}
