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
    public class carritoController : ControllerBase
    {
        private readonly ICarritoService _carritoService;
        public carritoController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
        }
        [Authorize]
        private int GetAuthenticatedUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Usuario no autenticado");
            }

            return userId;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllCarritos()
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                var carritos = await _carritoService.GetAllCarritos();
                return Ok(carritos);
            } catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
           
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarritoById(int id)
        {
            try
            {
                var carrito = await _carritoService.GetCarritoById(id);
                return Ok(carrito);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [Authorize]
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetCarritoByUsuarioId(int usuarioId)
        {
            try
            {
                var carrito = await _carritoService.GetCarritoByUsuarioId(usuarioId);
                return Ok(carrito);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCarrito([FromBody] CreationCarritoDto creationCarritoDto)
        {
            try
            {
                var carrito = await _carritoService.CreateCarrito(creationCarritoDto);
                return CreatedAtAction(nameof(GetCarritoById), new { id = carrito.Id }, carrito);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }


        [Authorize]
        [HttpPost("{carritoId}/items")]
        public async Task<IActionResult> AddItemToCarrito(int carritoId, [FromBody] AddItemCarritoRequest itemCarritoDto)
        {
            try
            {
                var carrito = await _carritoService.AddItemToCarrito(carritoId, itemCarritoDto.ProductoId, itemCarritoDto.Cantidad);
                return Ok(carrito);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize]

        [HttpPut("{carritoId}/items/{productoId}")]
        public async Task<IActionResult> UpdateItemQuantity(int carritoId, int productoId, [FromBody] UpdateQuantityRequest request)
        {
            try
            {
                var carrito = await _carritoService.UpdateItemQuantity(carritoId, productoId, request.NuevaCantidad);
                return Ok(carrito);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("{carritoId}/items/{productoId}")]
        public async Task<IActionResult> RemoveItemFromCarrito(int carritoId, int productoId)
        {
            try
            {
                var carrito = await _carritoService.RemoveItemFromCarrito(carritoId, productoId);
                return Ok(carrito);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> RemoveCarrito(int carritoId)
        {
            try
            {
                await _carritoService.DeleteCarrito(carritoId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }


           
        }

        [Authorize]
        [HttpDelete("{carritoId}/clear")]
        public async Task<IActionResult> ClearCarrito(int carritoId)
        {
            try
            {
                var carrito = await _carritoService.ClearCarrito(carritoId);
                return Ok(carrito);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
    public class AddItemRequest
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }

    public class UpdateQuantityRequest
    {
        public int NuevaCantidad { get; set; }
    }
}
