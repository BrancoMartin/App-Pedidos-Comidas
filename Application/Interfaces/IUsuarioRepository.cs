using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;


namespace Application.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<UsuarioDto> GetUserByIdAsync(int id);
        Task<IEnumerable<UsuarioDto>> GetAllUsersAsync();
        Task <UsuarioDto> CreateUser (UsuarioDto usuario);
        Task UpdateUserAsync(int id, UsuarioDto usuario);
        Task DeleteUserAsync(int id);
    }
}
