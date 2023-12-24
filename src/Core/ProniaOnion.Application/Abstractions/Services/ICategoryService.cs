using ProniaOnion.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Abstractions.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryGetItemDto>> GetCategoriesAsync(
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);

        Task<IEnumerable<CategoryGetItemDto>> GetOrderedCategoriesAsync(
            string orderBy,
            int? page = null,
            int? limit = null,
            bool isDescending = false,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);
       
        Task<CategoryGetDto> GetCategoryByIdAsync(int id, bool isTracking = false, bool showDeleted = false, params string[] includes);

        Task<IEnumerable<CategoryGetItemDto>> SearchCategoriesAsync(
            string searchTerm,
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);

        Task<CategoryGetDto> CreateCategoryAsync(CategoryPostDto categoryDto);



        Task<CategoryGetDto> UpdateCategoryAsync(int id, CategoryPutDto categoryDto);


        Task SoftDeleteCategoryAsync(int id);
        Task RevertSoftDeleteCategoryAsync(int id);

        Task DeleteCategoryAsync(int id);

    }
}
