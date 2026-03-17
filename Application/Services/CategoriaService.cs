using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoryRepository _categoriaRepository;
        public CategoriaService(ICategoryRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }


        public async Task<CategoriaDto> GetCategoriaById(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
            {
                throw new NotFoundException($"Categoría con id:{id} no fue encontrada.");
            }
            return CategoriaDto.CreateCategoria(categoria);
        }


        public async Task<IEnumerable<CategoriaDto>> GetAllCategorias()
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            return CategoriaDto.CreateList(categorias);
        }


        public async Task<CategoriaDto> CreateCategoria(CreationCategoryDto creationCategoryDto)
        {
            var newCategoria = new Categoria();

            newCategoria.NombreCategoria = creationCategoryDto.NombreCategoria; 
            
            var existingCategoria = await _categoriaRepository.GetCategoryByName(newCategoria.NombreCategoria);

            if (existingCategoria != null)
            {
                throw new ValidationException($"Categoría con nombre:{newCategoria.NombreCategoria} ya existe.");
            }

            var createdCategoria = await _categoriaRepository.CreateAsync(newCategoria);
            
            return CategoriaDto.CreateCategoria(createdCategoria);
        }

        public async Task UpdateCategoria(int id, CreationCategoryDto creationCategoryDto)
        {
            var categoriaToUpdate = await _categoriaRepository.GetByIdAsync(id);
            if (categoriaToUpdate == null)
            {
                throw new NotFoundException($"Categoría con id:{id} no fue encontrada.");
            }
            categoriaToUpdate.NombreCategoria = creationCategoryDto.NombreCategoria;
            await _categoriaRepository.UpdateAsync(categoriaToUpdate);
        }

        public async Task DeleteCategoria(int id)
        {
            var existingCategory = await _categoriaRepository.GetByIdAsync(id);
            
            if (existingCategory == null)
            {
                throw new NotFoundException($"Categoría con id:{id} no fue encontrada.");
            }
            await _categoriaRepository.DeleteAsync(existingCategory);
        }
    }
}
