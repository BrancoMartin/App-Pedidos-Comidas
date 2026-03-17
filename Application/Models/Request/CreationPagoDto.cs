using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationPagoDto
    {
        //[Required (ErrorMessage = "El Id del pedido es requerido")]
        //public int PedidoId { get; set; }
        [Required (ErrorMessage = "El Id del metodo de pago es requerido")]
        public int MetodoPagoId { get; set; }

    }

    
    public class UpdateEstadoPagoDto
    {
        [Required(ErrorMessage = "El estado del pago es requerido")]
        public EstadoPago EstadoPago { get; set; }
    }
}
