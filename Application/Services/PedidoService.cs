using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IUserRepository _usuarioRepository;
        private readonly ICarritoRepository _carritoRepository;
        public PedidoService(
            IPedidoRepository pedidoRepository,
            IProductoRepository productoRepository,
            IUserRepository usuarioRepository,
            ICarritoRepository carritoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _productoRepository = productoRepository;
            _usuarioRepository = usuarioRepository;
            _carritoRepository = carritoRepository;
        }

        public async Task<PedidoDto> GetPedidoById(int id)
        {
            var pedido = await _pedidoRepository.GetPedidoByIdAsync(id);
            if (pedido == null)
            {
                throw new NotFoundException($"Pedido con id:{id} no fue encontrado.");
            }
            return PedidoDto.CreatePedido(pedido);
        }

        public async Task<IEnumerable<PedidoDto>> GetAllPedidos()
        {
            var pedidos = await _pedidoRepository.GetAllPedidosAsync();
            
            var pedidos_ = pedidos.Select(PedidoDto.CreatePedido).ToList();
            
            return pedidos_;
        }



        public async Task<PedidoDto> CreatePedido(CreationPedidoDto creationPedidoDto)
        {
            // Validar que el usuario existe
            //  var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var usuario = await _usuarioRepository.GetByIdAsync(creationPedidoDto.UsuarioId);
            if (usuario == null)
            {
                throw new NotFoundException($"Usuario con id:{creationPedidoDto.UsuarioId} no existe.");
            }

            
            decimal precioTotal = 0;
            var itemsPedido = new List<ItemPedido>();

        
            var carrito = await _carritoRepository.GetByIdAsync(creationPedidoDto.CarritoId);

            if(carrito == null)
            {
                throw new NotFoundException($"Carrito con id:{creationPedidoDto.CarritoId} no fue encontrado.");
            }

            
            foreach (var itemDto in carrito.Items)
            {
                var producto = await _productoRepository.GetByIdAsync(itemDto.ProductoId);
                if (producto == null)
                {
                    throw new NotFoundException($"Producto con id:{itemDto.ProductoId} no existe.");
                }

                var itemPedido = new ItemPedido
                {
                    ProductoId = itemDto.ProductoId,
                    Cantidad = itemDto.Cantidad,
                    PrecioUnitario = producto.Precio
                };

                precioTotal += itemPedido.PrecioUnitario * itemPedido.Cantidad;
                itemsPedido.Add(itemPedido);
            }

            
            var newPedido = new Pedido
            {
                UsuarioId = creationPedidoDto.UsuarioId,
                Direccion = creationPedidoDto.Direccion,
                TiempoEstimado = "30-45 min",
                PrecioTotal = precioTotal,
                EstadoPedido = EstadoPedido.Pendiente, // Siempre empieza como Pendiente
                ItemsPedido = itemsPedido
            };

            var createdPedido = await _pedidoRepository.CreateAsync(newPedido);

            // 7. Limpiar el carrito (importante!)
            await _carritoRepository.DeleteAsync(carrito);

            return PedidoDto.CreatePedido(createdPedido);
        }




        public async Task<IEnumerable<PedidoDto>> GetPedidosByUsuarioId(int usuarioId)
        {
            var pedidos = await _pedidoRepository.GetByUserIdAsync(usuarioId);
            return PedidoDto.CreateList(pedidos);
        }




        public async Task<IEnumerable<PedidoDto>> GetPedidosByEstado(EstadoPedido estado)
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            var pedidosFiltrados = pedidos.Where(p => p.EstadoPedido == estado);
            return pedidosFiltrados.Select(PedidoDto.CreatePedido).ToList();
        }

       

        public async Task UpdateEstadoPedido(int id, EstadoPedido nuevoEstado)
        {
            var pedido = await _pedidoRepository.GetPedidoByIdAsync(id);
            if (pedido == null)
            {
                throw new NotFoundException($"Pedido con id:{id} no fue encontrado.");
            }

            pedido.EstadoPedido = nuevoEstado;
            await _pedidoRepository.UpdateAsync(pedido);
        }

        public async Task DeletePedido(int id)
        {
            var pedido = await _pedidoRepository.GetPedidoByIdAsync(id);
            if (pedido == null)
            {
                throw new NotFoundException($"Pedido con id:{id} no fue encontrado.");
            }

            // Solo poder eliminar si el estado es Entregado o Cancelado
            if (pedido.EstadoPedido == EstadoPedido.Entregado ||
                pedido.EstadoPedido == EstadoPedido.Cancelado)
            {
                await _pedidoRepository.DeleteAsync(pedido);
            }
            else
            {
                throw new InvalidOperationException(
                    "Solo se pueden eliminar pedidos Entregados o Cancelados.");
            }
        }


      
        //agregar item al pedido. si ese producto ya tiene un item en el pedido, se suma la cantidad  
        public async Task<PedidoDto> AddItemToPedido(int pedidoId, CreateItemPedidoDto itemPedido)
        {
            if (itemPedido.Cantidad <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor que cero.");
            }

            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido == null)
            {
                throw new NotFoundException($"pedido con id:{pedidoId} no fue encontrado.");
            }

            var producto = await _productoRepository.GetByIdAsync(itemPedido.ProductoId);
            if (producto == null)
            {
                throw new NotFoundException($"Producto con id:{itemPedido.ProductoId} no fue encontrado.");
            }

            var itemExistente = pedido.ItemsPedido.FirstOrDefault(i => i.ProductoId == itemPedido.ProductoId);

            if (itemExistente != null)
            {
                itemExistente.Cantidad += itemPedido.Cantidad;
            }
            else
            {
                // Agregar nuevo item
                var nuevoItem = new ItemPedido
                {
                    PedidoId = pedidoId,
                    ProductoId = itemPedido.ProductoId,
                    Cantidad = itemPedido.Cantidad,
                    PrecioUnitario = producto.Precio
                };
                pedido.ItemsPedido.Add(nuevoItem);
            }


            await _pedidoRepository.UpdateAsync(pedido);
            return PedidoDto.CreatePedido(pedido);
        }
    }
}
