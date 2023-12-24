using ProniaOnion.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Abstractions.Services
{
    public interface IProductService
    {
        Task CreateProductAsync(ProductPostDto productDto);
        Task<IEnumerable<ProductGetItemDto>> GetProductsAsync(
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);

        Task<IEnumerable<ProductGetItemDto>> GetOrderedProductsAsync(
            string orderBy,
            int? page = null,
            int? limit = null,
            bool isDescending = false,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);
        

        Task<IEnumerable<ProductGetItemDto>> SearchProductsAsync(
            string searchTerm,
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);

        Task<ProductGetDto> GetProductByIdAsync(
            int id,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);



        Task UpdateProductAsync(int id, ProductPutDto productDto);

        Task DeleteProductAsync(int id);

        Task SoftDeleteProductAsync(int id);

        Task RevertSoftDeleteProductAsync(int id);
    }
}
