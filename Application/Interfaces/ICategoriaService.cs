using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Request;

namespace Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<CategoriaDto> GetCategoriaById(int id);
        Task<IEnumerable<CategoriaDto>> GetAllCategorias();
        Task<CategoriaDto> CreateCategoria(CreationCategoryDto creationCategoryDto);
        Task UpdateCategoria(int id, CreationCategoryDto creationCategoryDto);
        Task DeleteCategoria(int id);

    }
}
