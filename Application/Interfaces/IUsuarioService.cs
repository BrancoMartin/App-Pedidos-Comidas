using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDto?> GetUsuarioById(int id);
        Task<IEnumerable<UsuarioDto>> GetAllUsuarios();
        Task<UsuarioDto> CreateUsuario(CreationUserDto creationuserDto);
        Task UpdateUsuario(int id, UpdateUserDto updateUserDto);
        Task DeleteUsuario(int id);
    }
}
