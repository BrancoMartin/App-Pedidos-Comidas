using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entities
{
    public class MetodoPago
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public TipoMetodoPago TipoMetodo { get; set; }
        public string? Tarjeta { get; set; }
        public string Detalles { get; set; } 

        // Relaciones
        public ICollection<Pago> Pagos { get; set; } = new List<Pago>();

        public List<MetodoPago> ToList()
        {
            throw new NotImplementedException();
        }
    }
}
