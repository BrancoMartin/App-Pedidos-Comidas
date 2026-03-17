using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;
using Domain.Interfaces;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUserRepository _usuarioRepository;
        private readonly ICarritoRepository _carritoRepository;
        public UsuarioService(IUserRepository usuarioRepository, ICarritoRepository carritoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _carritoRepository = carritoRepository;
        }

        public async Task<UsuarioDto?> GetUsuarioById(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);

            if (usuario == null)
            {
                throw new NotFoundException($"Usuario con id:{id} no fue encontrado.");
            }

            return UsuarioDto.CreateUser(usuario);
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllUsuarios()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return UsuarioDto.CreateList(usuarios);
        }

        public async Task<UsuarioDto> CreateUsuario(CreationUserDto creationuserDto)
        {
            var newUsuario = new Usuario
            {
                Nombre = creationuserDto.Nombre,
                Contraseña = creationuserDto.Contraseña, 
                Apellido = creationuserDto.Apellido,
                Telefono = creationuserDto.Telefono,
                Rol = RolUsuario.User 
            };

            var existingUsuario = await _usuarioRepository.GetUserByPhoneAsync(newUsuario.Telefono);

            if (existingUsuario != null)
            {
                throw new UserAlreadyRegisterException($"El teléfono {newUsuario.Telefono} ya está registrado.");
            }

            var createdUsuario = await _usuarioRepository.CreateAsync(newUsuario);

            // Crear carrito vacio automáticamente
            var carrito = new Carrito
            {
                UsuarioId = createdUsuario.Id,
                Items = new List<ItemCarrito>()
            };
            await _carritoRepository.CreateAsync(carrito); 

            return UsuarioDto.CreateUser(createdUsuario);
        }

        public async Task UpdateUsuario(int id, UpdateUserDto updateUserDto)
        {
            var usuarioToUpdate = await _usuarioRepository.GetByIdAsync(id);
            if (usuarioToUpdate == null)
            {
                throw new NotFoundException($"Usuario con id:{id} no fue encontrado.");
            }

            usuarioToUpdate.Nombre = updateUserDto.Nombre;
            usuarioToUpdate.Apellido = updateUserDto.Apellido;
            usuarioToUpdate.Telefono = updateUserDto.Telefono;
            usuarioToUpdate.Contraseña = updateUserDto.Contraseña;

            await _usuarioRepository.UpdateAsync(usuarioToUpdate);
        }

        public async Task DeleteUsuario(int id)
        {
            var existingUsuario = await _usuarioRepository.GetByIdAsync(id);
            if (existingUsuario == null)
            {
                throw new NotFoundException($"Usuario con id:{id} no fue encontrado.");
            }
            await _usuarioRepository.DeleteAsync(existingUsuario); 
        }
    }
}
