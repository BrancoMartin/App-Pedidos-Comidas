using Application.Models;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMetodoPagoService
    {
        Task<MetodoPagoDto?> GetMetodoPagoById(int id);
        Task<IEnumerable<MetodoPagoDto>>GetAllMetodoPago();
        Task<MetodoPagoDto> CreateMetodoPago(CreationMetodoPagoDto metodoPago);
        Task UpdateMetodoPago(int id, CreationMetodoPagoDto metodoPago);
        Task DeleteMetodoPago(int id);  

        Task<IEnumerable<MetodoPagoDto>> GetMetodoPagoByUserIdAsync(int usuarioId);
    }
}
