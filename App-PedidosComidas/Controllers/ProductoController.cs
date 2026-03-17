using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App_PedidosComidas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class productoController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly ICategoriaService _categoriaService;
        public productoController(IProductoService productoService, ICategoriaService categoriaService)
        {
            _productoService = productoService;
            _categoriaService = categoriaService;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductoById(int id, [FromQuery] string? currency)
        {
            try
            {
                var producto = await _productoService.GetProductoById(id, currency);
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllProductos([FromQuery] string? currency)
        {
            try
            {
                var productos = await _productoService.GetAllProductos(currency);
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

       

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProducto([FromBody] CreationProductoDto creationProductoDto)
        {
            try
            {
                var producto = await _productoService.CreateProducto(creationProductoDto);
                return CreatedAtAction(nameof(GetProductoById), new { id = producto.Id }, producto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, [FromBody] CreationProductoDto creationProductoDto)
        {
            try
            {
                await _productoService.UpdateProducto(id, creationProductoDto);
                return Ok(creationProductoDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            try
            {
                await _productoService.DeleteProducto(id);
                return Ok(); 
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}