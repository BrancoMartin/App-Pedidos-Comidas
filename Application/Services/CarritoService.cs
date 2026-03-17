using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly ICarritoRepository _carritoRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IUserRepository _usuarioRepository;

        public CarritoService(
            ICarritoRepository carritoRepository,
            IProductoRepository productoRepository,
            IUserRepository usuarioRepository)
        {
            _carritoRepository = carritoRepository;
            _productoRepository = productoRepository;
            _usuarioRepository = usuarioRepository;
        }


        public async Task<CarritoDto?> GetCarritoByUsuarioId(int usuarioId)
        {
            var carrito = await _carritoRepository.GetByUserIdAsync(usuarioId);
            if (carrito == null)
            {
                throw new NotFoundException($"Carrito para el usuario con id:{usuarioId} no fue encontrado.");
            }
            return CarritoDto.CreateCarrito(carrito);
        }

        public async Task<CarritoDto> CreateCarrito(CreationCarritoDto creationCarritoDto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(creationCarritoDto.UsuarioId);
            if (usuario == null)
            {
                throw new NotFoundException($"Usuario con id:{creationCarritoDto.UsuarioId} no fue encontrado.");
            }

            // Verificar si ya existe un carrito para este usuario
            //VALIDACION
            var carritoExistente = await _carritoRepository.GetByUserIdAsync(creationCarritoDto.UsuarioId);
            if (carritoExistente != null)
            {
                throw new InvalidOperationException($"El usuario con id:{creationCarritoDto.UsuarioId} ya tiene un carrito.");
            }
            var newCarrito = new Carrito
            {
                UsuarioId = creationCarritoDto.UsuarioId,
                Items = new List<ItemCarrito>()
            };

            if (creationCarritoDto.Items != null && creationCarritoDto.Items.Count > 0)
            {
                foreach (var itemDto in creationCarritoDto.Items)
                {
                    // Validar que el producto existe
                    var producto = await _productoRepository.GetByIdAsync(itemDto.ProductoId);
                    if (producto == null)
                    {
                        throw new NotFoundException($"El producto con id:{itemDto.ProductoId} no existe");
                    }

                    var newItem = new ItemCarrito
                    {
                        ProductoId = itemDto.ProductoId,
                        Cantidad = itemDto.Cantidad

                    };

                    newCarrito.Items.Add(newItem);
                }
            }

            var createdCarrito = await _carritoRepository.CreateAsync(newCarrito);

            return CarritoDto.CreateCarrito(createdCarrito);
        }

        public async Task DeleteCarrito(int id)
        {
            var carritoToDelete = await _carritoRepository.GetByIdAsync(id);
            if (carritoToDelete == null)
            {
                throw new NotFoundException($"Carrito con id:{id} no fue encontrado.");
            }
            await _carritoRepository.DeleteAsync(carritoToDelete);
        }

        public async Task<IEnumerable<CarritoDto>> GetAllCarritos()
        {
            var carritos = await _carritoRepository.GetAllCarritosAsync();
            return CarritoDto.CreateList(carritos);
        }

        public async Task<CarritoDto?> GetCarritoById(int id)
        {
            var carrito = await _carritoRepository.GetByIdAsync(id);
            if (carrito == null)
            {
                throw new NotFoundException($"Carrito con id:{id} no fue encontrado."); ;
            }
            return CarritoDto.CreateCarrito(carrito); 
        }

        //Metodos para items del carrito
        public async Task<CarritoDto> AddItemToCarrito(int carritoId, int productoId, int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor que cero.");
            }

            var carrito = await _carritoRepository.GetByIdAsync(carritoId);
            if (carrito == null)
            {
                throw new NotFoundException($"Carrito con id:{carritoId} no fue encontrado.");
            }

            var producto = await _productoRepository.GetByIdAsync(productoId);
            if (producto == null )
            {
                throw new NotFoundException($"Producto con id:{productoId} no fue encontrado.");
            }

            var itemExistente = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);

            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                // Agregar nuevo item
                var nuevoItem = new ItemCarrito
                {
                    CarritoId = carritoId,
                    ProductoId = productoId,
                    Cantidad = cantidad,
                };
                carrito.Items.Add(nuevoItem);
            }


            await _carritoRepository.UpdateAsync(carrito);
            return CarritoDto.CreateCarrito(carrito);
        }

        public async Task<CarritoDto> UpdateItemQuantity(int carritoId, int productoId, int nuevaCantidad)
        {
            if (nuevaCantidad <= 0)
            {
                throw new BadRequestException("La cantidad debe ser mayor a 0.");
            }

            var carrito = await _carritoRepository.GetByIdAsync(carritoId);
            if (carrito == null)
            {
                throw new NotFoundException($"Carrito con id:{carritoId} no fue encontrado.");
            }

            var item = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);
            if (item == null)
            {
                throw new NotFoundException($"Producto con id:{productoId} no está en el carrito.");
            }

            var producto = await _productoRepository.GetByIdAsync(productoId);
            if (producto == null)
            {
                throw new NotFoundException($"Producto con id:{productoId} no fue encontrado.");
            }

            item.Cantidad = nuevaCantidad;

            await _carritoRepository.UpdateAsync(carrito);
            return CarritoDto.CreateCarrito(carrito);
        }

        public async Task<CarritoDto> RemoveItemFromCarrito(int carritoId, int productoId)
        {
            var carrito = await _carritoRepository.GetByIdAsync(carritoId);
            if (carrito == null)
            {
                throw new NotFoundException($"Carrito con id:{carritoId} no fue encontrado.");
            }

            var item = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);
            if (item == null)
            {
                throw new NotFoundException($"Producto con id:{productoId} no está en el carrito.");
            }

            carrito.Items.Remove(item);

            await _carritoRepository.UpdateAsync(carrito);
            return CarritoDto.CreateCarrito(carrito);
        }

        public async Task<CarritoDto> ClearCarrito(int carritoId)
        {
            var carrito = await _carritoRepository.GetByIdAsync(carritoId);
            if (carrito == null)
            {
                throw new NotFoundException($"Carrito con id:{carritoId} no fue encontrado.");
            }

            carrito.Items.Clear();

            await _carritoRepository.UpdateAsync(carrito);
            return CarritoDto.CreateCarrito(carrito);
        }


    }
}
