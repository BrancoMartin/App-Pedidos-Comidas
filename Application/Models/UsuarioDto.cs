using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; }

        public static UsuarioDto CreateUser(Usuario usuario)
        {
            var dto = new UsuarioDto();
            dto.Id = usuario.Id;
            dto.Nombre = usuario.Nombre;
            dto.Apellido = usuario.Apellido;
            dto.Telefono = usuario.Telefono;
            dto.Rol = usuario.Rol;
            return dto;
        }

        public static List<UsuarioDto> CreateList(List<Usuario> userList)
        {
            var dtoList = new List<UsuarioDto>();
            foreach (var u in userList)
            {
                dtoList.Add(CreateUser(u));
            }
            return dtoList;
        }
    }
};

