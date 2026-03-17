using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



namespace App_PedidosComidas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class pagoController : ControllerBase
    {
        private readonly IPagoService _pagoService;
        public pagoController(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPagoById(int id)
        {
            try
            {
                var pago = await _pagoService.GetPagoById(id);
                return Ok(pago);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [Authorize(Roles ="Admin")]

        [HttpGet]
        public async Task<IActionResult> GetAllPagos()
        {
            try
            {
                var pagos = await _pagoService.GetAllPagos();
                return Ok(pagos);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [Authorize]
        [HttpPost("pedido/{pedidoId}")]
        public async Task<IActionResult> CreatePago([FromBody] CreationPagoDto creationPagoDto, int pedidoId)
        {
            try
            {
                var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // User nos permite acceder a los claims del jwt 
                if (string.IsNullOrEmpty(usuarioIdClaim))
                    return Unauthorized();

                var usuarioId = int.Parse(usuarioIdClaim);

                Console.WriteLine($"Usuario autenticado: {usuarioId}");
                Console.WriteLine($"Pedido solicitado: {pedidoId}");

                var pago = await _pagoService.CreatePago(creationPagoDto, pedidoId, usuarioId);
                return CreatedAtAction(nameof(GetAllPagos), new { id = pago.Id }, pago);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdatePago(int id, [FromBody] UpdateEstadoPagoDto updateEstadoPagoDto)
        {
            try
            {
                await _pagoService.UpdatePago(id, updateEstadoPagoDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            try
            {
                await _pagoService.DeletePago(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [Authorize]
        [HttpGet("ByPedido/{pedidoId}")]
        public async Task<IActionResult> GetPagosByPedidoId(int pedidoId)
        {
            try
            {
                var pagos = await _pagoService.GetPagosByPedidoId(pedidoId);
                return Ok(pagos);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("ByUsuario/{usuarioId}")]
        public async Task<IActionResult> GetPagosByUsuarioId(int usuarioId)
        {
            try
            {
                var pagos = await _pagoService.GetPagosByUsuarioId(usuarioId);
                return Ok(pagos);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        
    }
}