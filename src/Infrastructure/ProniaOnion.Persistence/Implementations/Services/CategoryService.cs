using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ProniaOnion.Application.Abstractions.Repositories;
using ProniaOnion.Application.Abstractions.Services;
using ProniaOnion.Application.Dtos;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Persistence.Implementations.Services
{
    public class CategoryService : ICategoryService
    {
        private const int LIMIT = 5;
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CategoryGetDto> CreateCategoryAsync(CategoryPostDto categoryDto)
        {

            Category newCategory = _mapper.Map<Category>(categoryDto);
            await _repository.CreateAsync(newCategory);
            await _repository.SaveChangesAsync();
            return _mapper.Map<CategoryGetDto>(newCategory);
        }


        public async Task<IEnumerable<CategoryGetItemDto>> GetCategoriesAsync(
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes
        )
        {
            int? skip = _getSkip(page, limit); 
            IEnumerable<Category> categories = await _repository.GetAll( 
                 skip,
                 limit is null && page is not null ? LIMIT : limit,
                 isTracking,
                 showDeleted,
                 includes).ToListAsync();

            return _mapper.Map<IEnumerable<CategoryGetItemDto>>(categories);
        }


        public async Task<IEnumerable<CategoryGetItemDto>> GetOrderedCategoriesAsync(
            string orderBy,
            int? page = null,
            int? limit = null,
            bool isDescending = false,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {
            Expression<Func<Category, object>>? expression = _getOrderExpression(orderBy)
                ?? throw new Exception("bad request");
            int? skip = _getSkip(page, limit);
            IEnumerable<Category> categories = await _repository.OrderAndGet(
                expression,
                isDescending,
                skip,
                limit is null && page is not null ? LIMIT : limit,
                isTracking,
                showDeleted,
                includes).ToListAsync();

            return _mapper.Map<ICollection<CategoryGetItemDto>>(categories);
        }
        public async Task<IEnumerable<CategoryGetItemDto>> SearchCategoriesAsync(
            string searchTerm,
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {

            Expression<Func<Category, bool>> expression = c => c.Name.ToLower().Contains(searchTerm.ToLower());
            int? skip = _getSkip(page, limit);
            IEnumerable<Category> categories = await _repository.SearchAndGet(
                expression,
                skip,
                limit is null && page is not null ? LIMIT : limit,
                isTracking,
                showDeleted,
                includes).ToListAsync();

            return _mapper.Map<ICollection<CategoryGetItemDto>>(categories);

        }

        public async Task<CategoryGetDto> GetCategoryByIdAsync(
            int id,
            bool isTracking = false,
            bool showDeleted=false,
            params string[] includes)
        {
            Category category = await _repository.GetByIdAsync(id, isTracking,showDeleted, includes) 
                ?? throw new Exception("Category wasnt found!");
            return _mapper.Map<CategoryGetDto>(category);
            

        }

        public async Task<CategoryGetDto> UpdateCategoryAsync(int id, CategoryPutDto categoryDto)
        {
            Category category = await _repository.GetByIdAsync(id, true) ?? throw new Exception("Category wasnt found!");
            category.Name = categoryDto.Name;
            await _repository.SaveChangesAsync();
            return _mapper.Map<CategoryGetDto>(category);

        }
        public async Task SoftDeleteCategoryAsync(int id)
        {
            Category cat = await _repository.GetByIdAsync(id, true) ?? throw new Exception("Category wasnt found!");
            if (!cat.IsDeleted)
            {
                _repository.SoftDelete(cat);
                await _repository.SaveChangesAsync();
            }

        }
        public async Task RevertSoftDeleteCategoryAsync(int id)
        {
            Category cat = await _repository.GetByIdAsync(id, true, true) ?? throw new Exception("Category wasnt found!");
            if (cat.IsDeleted)
            {
                _repository.RevertSoftDelete(cat);
                await _repository.SaveChangesAsync();
            }

        }
        public async Task DeleteCategoryAsync(int id)
        {
            Category category = await _repository.GetByIdAsync(id,showDeleted:true) ?? throw new Exception("Category wasnt found!");
            _repository.Delete(category);
            await _repository.SaveChangesAsync();
        }


        // get expression for order method
        private Expression<Func<Category, object>> _getOrderExpression(string orderBy)
        {
            Expression<Func<Category, object>>? expression = null;
            switch (orderBy.ToLower())
            {
                case "name":
                    expression = c => c.Name;
                    break;
                case "id":
                    expression = c => c.Id;
                    break;
            }

            return expression;
        }

        private int? _getSkip(int? page, int? limit)
        {
            int? skip = null;
            if (page > 0)
            {

                if (limit is not null) skip = ((page - 1) * limit);
                else skip = ((page - 1) * LIMIT);
            }
            return skip;
        }

    }
}
