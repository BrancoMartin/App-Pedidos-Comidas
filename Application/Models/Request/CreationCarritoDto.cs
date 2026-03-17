using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationCarritoDto
    {
        [Required(ErrorMessage = "El Id del usuario es requerido")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Debe incluir al menos un producto")]
        public List<ItemCarritoDto> Items { get; set; } = new List<ItemCarritoDto>();


        public class CreateItemCarritoDto
        {
            [Required (ErrorMessage = "El ID del carrito es requerido")]
            public int CarritoId { get; set; }

            [Required(ErrorMessage = "El ID del producto es requerido")]
            public int ProductoId { get; set; }

            [Required(ErrorMessage = "La Cantidad es requerida")]
            [Range(1, 30, ErrorMessage = "La cantidad debe estar entre 1 y 30")]
            public int Cantidad { get; set; }


        }
    }
}
