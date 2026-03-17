using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Application.Models
{
    public class MetodoPagoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public TipoMetodoPago TipoMetodo { get; set; }
        public string Detalles { get; set; }

        public static MetodoPagoDto CreateMetodoPago(MetodoPago metodoPago)
        {
            var dto = new MetodoPagoDto();
            dto.Id = metodoPago.Id;
            dto.UsuarioId = metodoPago.UsuarioId;
            dto.TipoMetodo = metodoPago.TipoMetodo;
            dto.Detalles = metodoPago.Detalles;
            return dto;
        }

        internal static MetodoPagoDto? CreateMetodoPago(Task<MetodoPago> metodoPago)
        {
            throw new NotImplementedException();
        }

        public static List<MetodoPagoDto> CreateList(List<MetodoPago> metodoPagoList)
        {
            var dtoList = new List<MetodoPagoDto>();
            foreach (var p in metodoPagoList)
            {
                dtoList.Add(CreateMetodoPago(p));
            }
            return dtoList;
        }
    }
}
