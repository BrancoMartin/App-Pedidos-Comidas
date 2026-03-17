using Application.Interfaces;
using Application.Services;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Services
{
    public class ProductoService : IProductoService
    {
        // Inyeccion de dependencias
        private readonly IProductoRepository _productoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrencyService _currencyService;

        public ProductoService(IProductoRepository productoRepository, ICategoryRepository categoryRepository, ICurrencyService currencyService)
        {
            _productoRepository = productoRepository;
            _categoryRepository = categoryRepository;
            _currencyService = currencyService;
        }

        public async Task<ProductoDto> GetProductoById(int id, string? currency = null)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null)
            {
                throw new NotFoundException($"Producto con id:{id} no fue encontrado.");
            }

            var productoDto = ProductoDto.CreateProducto(producto);

            await ApplyConversionToProducto(productoDto, currency);
            
            return productoDto;
        }

        public async Task<IEnumerable<ProductoDto>> GetAllProductos(string? currency = null)
        {
            var productos = await _productoRepository.GetAllAsync();

            // Convertir entidades a DTOs primero
            var productoDtos = productos.Select(ProductoDto.CreateProducto).ToList();

            // Aplicar conversión de moneda a cada DTO (ApplyConversionToProducto espera un ProductoDto)
            var conversionTasks = productoDtos.Select(p => ApplyConversionToProducto(p, currency));
            await Task.WhenAll(conversionTasks);

            return productoDtos;
        }
        public async Task<ProductoDto> CreateProducto(CreationProductoDto creationProductoDto)
        {
            var category = await _categoryRepository.GetByIdAsync(creationProductoDto.CategoriaId);

            if (category is null)
            {
                throw new ValidationException("El id de la categoría es inválido");
            }

            if (creationProductoDto.Precio <= 0)
            {
                throw new ValidationException("El precio del producto debe ser mayor a cero");
            }


            var newProducto = new Producto();

            newProducto.Nombre = creationProductoDto.Nombre;
            newProducto.Precio = creationProductoDto.Precio;
            newProducto.CategoriaId = creationProductoDto.CategoriaId;


            var createdProducto = await _productoRepository.CreateAsync(newProducto);

            return ProductoDto.CreateProducto(createdProducto);
        }

        public async Task UpdateProducto(int id, CreationProductoDto creationProductoDto)
        {
            var productoToUpdate = await _productoRepository.GetByIdAsync(id);
            if (productoToUpdate == null)
            {
                throw new NotFoundException($"Producto con id:{id} no fue encontrado.");
            }
          
            productoToUpdate.Nombre = creationProductoDto.Nombre;
            productoToUpdate.Precio = creationProductoDto.Precio;
            productoToUpdate.CategoriaId = creationProductoDto.CategoriaId;

            await _productoRepository.UpdateAsync(productoToUpdate);
        }

        public async Task DeleteProducto(int id)
        {
            var existingProducto = await _productoRepository.GetByIdAsync(id);
            if (existingProducto == null)
            {
                throw new NotFoundException($"Producto con id:{id} no fue encontrado.");
            }
            await _productoRepository.DeleteAsync(existingProducto);
        }
        public async Task<ProductoDto> GetProductoByName(string? nombre, string? currency)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del producto no puede estar vacío.");
            }

            var productos = await _productoRepository.GetByNombreAsync(nombre.Trim());
            var producto = productos.FirstOrDefault();

            if (producto == null)
            {
                throw new NotFoundException($"Producto con nombre: {nombre.Trim()} no fue encontrado.");
            }
            var productoDto = ProductoDto.CreateProducto(producto);

            await ApplyConversionToProducto(productoDto, currency);

            return productoDto;
        }

        
        private async Task ApplyConversionToProducto(ProductoDto producto, string? currency)
        {
            if (!string.IsNullOrEmpty(currency) && currency.ToUpper() != "ARS")
            {
                try
                {
                    decimal exchangeRate = await _currencyService.GetExchangeRate("ARS", currency.ToUpper());
                    producto.PrecioConvertido = Math.Round(producto.Precio * exchangeRate, 2);
                    producto.Moneda = currency.ToUpper();
                }
                catch
                {
                    // Si falla la conversión, mantener ARS
                    producto.Moneda = "ARS";
                }
            }
            else
            {
                producto.Moneda = "ARS";
            }
        }

       
    }
}

