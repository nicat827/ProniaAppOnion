using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstractions.Repositories;
using ProniaOnion.Application.Abstractions.Services;
using ProniaOnion.Application.Dtos.Category;
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
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CategoryGetDto> CreateCategoryAsync(CategoryCreateDto categoryDto)
        {

            Category newCategory = new() { Name = categoryDto.Name };
            await _repository.CreateAsync(newCategory);
            await _repository.SaveChangesAsync();
            return _mapper.Map<CategoryGetDto>(newCategory);
        }

        public async Task DeleteCategory(int id)
        {
            Category category = await _repository.GetByIdAsync(id,showDeleted:true) ?? throw new Exception("Category wasnt found!");
            _repository.Delete(category);
            await _repository.SaveChangesAsync();


        }
        public async Task SoftDeleteCategoryAsync(int id)
        {
            Category cat = await _repository.GetByIdAsync(id) ?? throw new Exception("Category wasnt found!");
            if (!cat.IsDeleted)
            {
                cat.IsDeleted = true;
                _repository.SoftDelete(cat);
                await _repository.SaveChangesAsync();
            }

        }

        public async Task<ICollection<CategoryGetCollectionDto>> GetCategoriesAsync(
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes
        )
        {

            ICollection<Category> categories = new List<Category>();

            if (page is null)
            {
                if (limit is null) categories = await _repository.GetAll(isTracking: isTracking, includes: includes).ToListAsync();
                else categories = await _repository.GetAll(limit: (int)limit, isTracking: isTracking, includes: includes).ToListAsync();
            }
            else
            {
                if (limit is null) limit = 5;
                categories = await _repository.GetAll((int)((page - 1) * limit), (int)limit, isTracking,false, includes).ToListAsync();

            }
            return _mapper.Map<ICollection<CategoryGetCollectionDto>>(categories);
        }

        public async Task<CategoryGetDto> GetCategoryByIdAsync(int id, bool isTracking = false,bool showDeleted=false, params string[] includes)
        {
            Category category = await _repository.GetByIdAsync(id, isTracking,showDeleted, includes) ?? throw new Exception("Category wasnt found!");
            return new CategoryGetDto(category.Id, category.Name);
            

        }

        public async Task<ICollection<CategoryGetCollectionDto>> GetOrderedCategoriesAsync(
            string orderBy,
            int? page = null,
            int? limit = null,
            bool isDescending = false,
            bool isTracking = false,
            params string[] includes)
        {
            Expression<Func<Category, object>>? expression = GetOrderExpression(orderBy)
                ?? throw new Exception("bad request");
            ICollection<Category> categories = new List<Category>();

            if (page == null)
            {
                if (limit == null) categories = await _repository.OrderAndGet(order: expression, isDescending: isDescending, isTracking: isTracking, includes: includes).ToListAsync();
                else categories = await _repository.OrderAndGet(order: expression, isDescending: isDescending, limit: (int)limit, isTracking: isTracking, includes: includes).ToListAsync();
            }
            else
            {
                if (limit == null) limit = 5;
                categories = await _repository.OrderAndGet(order: expression, isDescending: isDescending, skip: (int)((page - 1) * limit), (int)limit, isTracking,false, includes).ToListAsync();

            }

            return _mapper.Map<ICollection<CategoryGetCollectionDto>>(categories);
        }
        public async Task<ICollection<CategoryGetCollectionDto>> SearchCategoriesAsync(string searchTerm, int? page = null, int? limit = null, bool isTracking = false, params string[] includes)
        {

            ICollection<Category> categories = new List<Category>();
            Expression<Func<Category, bool>> expression = c => c.Name.ToLower().Contains(searchTerm.ToLower());
            if (page == null)
            {
                if (limit == null) categories = await _repository.SearchAndGet(expression: expression, isTracking: isTracking, includes: includes).ToListAsync();
                else categories = await _repository.SearchAndGet(expression: expression, limit: (int)limit, isTracking: isTracking, includes: includes).ToListAsync();
            }
            else
            {
                limit ??= 5;
                categories = await _repository.SearchAndGet(expression: expression, skip: (int)((page - 1) * limit), (int)limit, isTracking, includes).ToListAsync();

            }

            return _mapper.Map<ICollection<CategoryGetCollectionDto>>(categories);

        }


        public async Task<CategoryGetDto> UpdateCategoryAsync(int id, CategoryUpdateDto categoryDto)
        {
            Category category = await _repository.GetByIdAsync(id) ?? throw new Exception("Category wasnt found!");
            category.Name = categoryDto.Name;
            _repository.Update(category);
            await _repository.SaveChangesAsync();
            return new CategoryGetDto(category.Id, category.Name);

        }


      


        // get expression for order method
        public Expression<Func<Category, object>> GetOrderExpression(string orderBy)
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

    }
}
