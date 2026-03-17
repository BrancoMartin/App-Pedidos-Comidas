
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;


namespace Application.Models
{
    public class CarritoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        // Items del pedido
        public List<ItemCarritoDto> Items { get; set; } = new List<ItemCarritoDto>();

        public static CarritoDto CreateCarrito(Carrito carrito)
        {
            var dto = new CarritoDto();
            dto.Id = carrito.Id;
            dto.UsuarioId = carrito.UsuarioId;
            dto.Items = carrito.Items.Select(ItemCarritoDto.CreateItemCarrito).ToList();

            return dto;
        }

        public static List<CarritoDto> CreateList(List<Carrito> carritoList)
        {
            var dtoList = new List<CarritoDto>();
            foreach (var c in carritoList)
            {
                dtoList.Add(CreateCarrito(c));
            }
            return dtoList;
        }
    }

    public class ItemCarritoDto
    {
        public int Id { get; set; }
        public int CarritoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }

        public static ItemCarritoDto CreateItemCarrito(ItemCarrito item)
        {
            var dto = new ItemCarritoDto();
            dto.Id = item.Id;
            dto.CarritoId = item.CarritoId;
            dto.ProductoId = item.ProductoId;
            dto.Cantidad = item.Cantidad;
            return dto;
        }
    }
}
