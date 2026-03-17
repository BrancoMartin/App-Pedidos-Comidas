using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App_PedidosComidas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public userController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuarioById(int id)
        {
            var usuarioDto = await _usuarioService.GetUsuarioById(id);
            return Ok(usuarioDto);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetAllUsuarios();
            return Ok(usuarios);
        }



        [HttpPost("register")]
        public async Task<ActionResult<UsuarioDto>> CreateUsuario([FromBody] CreationUserDto creationUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarioDto = await _usuarioService.CreateUsuario(creationUserDto);
            return CreatedAtAction(nameof(GetUsuarioById), new { id = usuarioDto.Id }, usuarioDto);
        }


        

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuario(int id)
        {
            var usuarioDto = await _usuarioService.GetUsuarioById(id);
            if (usuarioDto == null)
            {
                return NotFound();
            }
            await _usuarioService.DeleteUsuario(id);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioDto>> UpdateUsuario(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            var existingUsuario = await _usuarioService.GetUsuarioById(id);
            if (existingUsuario == null)
            {
                return NotFound();
            }
            await _usuarioService.UpdateUsuario(id, updateUserDto);
            return Ok(existingUsuario);
        }

    }
}