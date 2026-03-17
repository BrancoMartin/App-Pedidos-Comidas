using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Services
{
    public class PagoService : IPagoService
    {
        private readonly IPagoRepository _pagoRepository;
        private readonly IMetodoPagoRepository _metodoPagoRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IUserRepository _userRepository;

        public PagoService(
                IPagoRepository pagoRepository,
                IMetodoPagoRepository metodoPagoRepository,
                IPedidoRepository pedidoRepository,
                IUserRepository userRepository)
        {
            _pagoRepository = pagoRepository;
            _metodoPagoRepository = metodoPagoRepository;
            _pedidoRepository = pedidoRepository;
            _userRepository = userRepository;
        }

        public async Task<PagoDto?> GetPagoById(int id)
        {
            var pago = await _pagoRepository.GetByIdAsync(id);
            return pago != null ? PagoDto.CreatePago(pago) : null;
        }

        public async Task<IEnumerable<PagoDto>> GetAllPagos()
        {
            var pagos = await _pagoRepository.GetAllAsync();
            return PagoDto.CreateList(pagos.ToList());
        }

        public async Task<PagoDto> CreatePago(CreationPagoDto creationPagoDto, int pedidoId, int usuarioId)
        {
            // Validar existencia de Pedido
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido == null)
                throw new NotFoundException($"Pedido con id {pedidoId} no encontrado.");

            // Validar existencia de Metodo de Pago
            var metodoPago = await _metodoPagoRepository.GetByIdAsync(creationPagoDto.MetodoPagoId);
            if (metodoPago == null || metodoPago.UsuarioId != usuarioId)
                throw new ForbiddenException("Método de pago inválido");

            pedido.EstadoPedido = EstadoPedido.Confirmado;

            var pago = new Pago
            {
                PedidoId = pedidoId,
                MetodoPagoId = creationPagoDto.MetodoPagoId,
                Fecha = DateTime.UtcNow,
                EstadoPago = EstadoPago.Completado
            };

            await _pagoRepository.CreateAsync(pago);

            return PagoDto.CreatePago(pago);
        }

        public async Task UpdatePago(int id, UpdateEstadoPagoDto updateEstadoPagoDto)
        {
            var pagoToUpdate = await _pagoRepository.GetByIdAsync(id);
            if (pagoToUpdate == null)
                throw new NotFoundException($"No se encontro pago con id{id}");

            pagoToUpdate.EstadoPago = updateEstadoPagoDto.EstadoPago;

            await _pagoRepository.UpdateAsync(pagoToUpdate);

        }

 
        public async Task DeletePago(int id)
        {
            var pago = await _pagoRepository.GetByIdAsync(id);
            if (pago == null)
                throw new NotFoundException($"No existe pago con id:{id}");

            if (pago.EstadoPago == EstadoPago.Completado)
                throw new NotFoundException("No se puede eliminar un pago completado.");

           await _pagoRepository.DeleteAsync(pago);
        }


        public async Task<IEnumerable<PagoDto>> GetPagosByPedidoId(int pedidoId)
        {
            var pagos = await _pagoRepository.GetPagosByPedidoIdAsync(pedidoId);
            return PagoDto.CreateList(pagos.ToList());
        }

        public async Task<IEnumerable<PagoDto>> GetPagosByUsuarioId(int usuarioId)
        {
            var pagos = await _pagoRepository.GetPagosByUserIdAsync(usuarioId);
            return PagoDto.CreateList(pagos.ToList());
        }

    }
}