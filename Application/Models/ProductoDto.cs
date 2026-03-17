using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;  

namespace Application.Models
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; }

        // Nuevas propiedades para conversión de moneda
        public decimal? PrecioConvertido { get; set; }
        public string Moneda { get; set; } = "ARS";

        public static ProductoDto CreateProducto(Producto producto)
        {
            var dto = new ProductoDto();
            dto.Id = producto.Id;
            dto.Nombre = producto.Nombre;
            dto.Precio = producto.Precio;
            dto.CategoriaId = producto.CategoriaId;
            return dto;

        }


        public static List<ProductoDto> CreateList(List<Producto> productList)
        {
            var dtoList = new List<ProductoDto>();

            foreach (var p in productList)
            {
                dtoList.Add(CreateProducto(p));
            }

            return dtoList;
        }

    }
}
