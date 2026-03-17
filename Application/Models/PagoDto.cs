using Domain.Entities;
using Domain.Enum;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PagoDto
    {
        public int Id { get; set; }
        public int MetodoPagoId { get; set; }

        public static PagoDto CreatePago(Pago pago)
        {
            var dto = new PagoDto();
            dto.Id = pago.Id;
            dto.MetodoPagoId = pago.MetodoPagoId;
            return dto;
        }

        public static List<PagoDto> CreateList(List<Pago> pagoList)
        {
            var dtoList = new List<PagoDto>();
            foreach (var p in pagoList)
            {
                dtoList.Add(CreatePago(p));
            }
            return dtoList;
        }
    }

}
