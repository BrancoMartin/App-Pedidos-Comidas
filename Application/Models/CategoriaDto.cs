using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;      

namespace Application.Models
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string NombreCategoria { get; set; }

        public static CategoriaDto CreateCategoria(Categoria categoria)
        {
            var dto = new CategoriaDto();
            dto.Id = categoria.Id;
            dto.NombreCategoria = categoria.NombreCategoria;
            return dto;
        }

        public static List<CategoriaDto> CreateList(List<Categoria> categoriaList)
        {
            var dtoList = new List<CategoriaDto>();
            foreach (var c in categoriaList)
            {
                dtoList.Add(CreateCategoria(c));
            }
            return dtoList;
        }
    }
}
