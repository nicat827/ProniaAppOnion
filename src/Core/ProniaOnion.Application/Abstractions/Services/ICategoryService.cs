using ProniaOnion.Application.Dtos.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Abstractions.Services
{
    public interface ICategoryService
    {
        Task<ICollection<CategoryGetCollectionDto>> GetCategoriesAsync(int? page = null, int? limit = null, bool isTracking = false, bool showDeleted = false, params string[] includes);

        Task<ICollection<CategoryGetCollectionDto>> GetOrderedCategoriesAsync(
            string orderBy,
            int? page = null,
            int? limit = null,
            bool isDescending = false,
            bool isTracking = false,
            params string[] includes);
        //Task<ICollection<GetCategoryDto>> GetFilteredCategoriesAsync(string name, int? page, int? limit, bool isTracking= false);
        Task<CategoryGetDto> GetCategoryByIdAsync(int id, bool isTracking = false, bool showDeleted = false, params string[] includes);

        Task<ICollection<CategoryGetCollectionDto>> SearchCategoriesAsync(
            string searchTerm,
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            params string[] includes);

        Task<CategoryGetDto> CreateCategoryAsync(CategoryCreateDto categoryDto);



        Task<CategoryGetDto> UpdateCategoryAsync(int id, CategoryUpdateDto categoryDto);

        Task DeleteCategory(int id);

        Task SoftDeleteCategoryAsync(int id);
    }
}
