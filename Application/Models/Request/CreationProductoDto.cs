using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationProductoDto
    {
        [Required(ErrorMessage ="El nombre del producto es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El precio del producto es requerido")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "La categoria del producto es requerida")]
        public int CategoriaId { get; set; }
    }
}
