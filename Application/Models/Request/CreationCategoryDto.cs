using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationCategoryDto
    {
        [Required(ErrorMessage = "El nombre de la categoria es requerido")]
        public string NombreCategoria { get; set; }
    }
}
