
﻿using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace App_PedidosComidas.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class metodoPagoController : ControllerBase
        {
            private readonly IMetodoPagoService _metodoPagoSerivce;

            public metodoPagoController(IMetodoPagoService metodoPagoService)
            {
                _metodoPagoSerivce = metodoPagoService;
            }



        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var metodoPago = await _metodoPagoSerivce.GetMetodoPagoById(id);

            if (metodoPago == null)
            {
                return NotFound();
            }

            return Ok(metodoPago);
        }


        [Authorize(Roles = "Admin")]                      
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var metodosPago = await _metodoPagoSerivce.GetAllMetodoPago();
            return Ok(metodosPago);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreationMetodoPagoDto createDto)
        {
            try
            {
                var metodoPago = await _metodoPagoSerivce.CreateMetodoPago(createDto);
                return CreatedAtAction(nameof(GetById), new { id = metodoPago.Id }, metodoPago);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, inner = ex.InnerException?.Message });
            }

        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] CreationMetodoPagoDto creationMetodoPagoDto)
        {
            try
            {
                await _metodoPagoSerivce.UpdateMetodoPago(id, creationMetodoPagoDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _metodoPagoSerivce.DeleteMetodoPago(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuarioId(int usuarioId)
        {
            var metodosPago = await _metodoPagoSerivce.GetMetodoPagoByUserIdAsync(usuarioId);
            return Ok(metodosPago);
        }

    }
} 


