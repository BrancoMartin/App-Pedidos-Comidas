using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Request;

namespace Application.Interfaces
{
    public interface IProductoService
    {
        Task<ProductoDto> GetProductoById(int id, string? currency = null);
        Task<IEnumerable<ProductoDto>> GetAllProductos(string? currency = null);
        Task<ProductoDto> CreateProducto(CreationProductoDto creationProductoDto);
        Task UpdateProducto(int id, CreationProductoDto creationProductoDto);
        Task DeleteProducto(int id);

        Task<ProductoDto> GetProductoByName(string? nombre, string? currency = null);
    }
}
