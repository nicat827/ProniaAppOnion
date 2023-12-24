using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstractions.Repositories;
using ProniaOnion.Application.Abstractions.Repositories.Generic;
using ProniaOnion.Application.Abstractions.Services;
using ProniaOnion.Application.Dtos;
using ProniaOnion.Domain.Entities;
using ProniaOnion.Domain.Entities.Common;
using System.Diagnostics;
using System.Linq.Expressions;

namespace ProniaOnion.Persistence.Implementations.Services
{
    internal class ProductService : IProductService
    {
        private const int LIMIT = 5;
        private readonly IProductRepository _repository;
        private readonly IColorRepository _colorRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductTagRepository _productTagRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository repository,
            IColorRepository colorRepository,
            ITagRepository tagRepository,
            ICategoryRepository categoryRepository,
            IProductTagRepository productTagRepository,
            IMapper mapper)
        {
            _repository = repository;
            _colorRepository = colorRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
            _productTagRepository = productTagRepository;
            _mapper = mapper;
        }
        public async Task CreateProductAsync(ProductPostDto productDto)
        {

            Product newProduct = _mapper.Map<Product>(productDto);
            Category category = await _categoryRepository.GetByIdAsync(productDto.CategoryId)
                ?? throw new Exception($"Category with {productDto.CategoryId} wasnt defined!");
            newProduct.SKU = Guid.NewGuid().ToString().Substring(10);
            if (productDto.TagIds is not null)
            {
                foreach (int tagId in productDto.TagIds)
                {
                    Tag findedTag = await _tagRepository.GetByIdAsync(tagId) 
                        ?? throw new Exception($"Tag with id {tagId} wasnt defined!");

                    newProduct.ProductTags.Add( new ProductTag {TagId = tagId });
                }
            }
            if (productDto.ColorIds is not null)
            {
                foreach (int colorId in productDto.ColorIds)
                {
                    Color color = await _colorRepository.GetByIdAsync(colorId)
                        ?? throw new Exception($"Color with id {colorId} wasnt defined!");

                    newProduct.ProductColors.Add(new ProductColor { ColorId = colorId });
                }
            }
            await _repository.CreateAsync(newProduct);
            await _repository.SaveChangesAsync();
         
        }

        public async Task<IEnumerable<ProductGetItemDto>> GetProductsAsync(
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes
        )
        {
            int? skip = _getSkip(page, limit);
            IEnumerable<Product> products = await _repository.GetAll(
                 skip,
                 limit is null ? LIMIT : limit,
                 isTracking,
                 showDeleted,
                 includes).ToListAsync();

            return _mapper.Map<IEnumerable<ProductGetItemDto>>(products);
        }


        public async Task<IEnumerable<ProductGetItemDto>> GetOrderedProductsAsync(
            string orderBy,
            int? page = null,
            int? limit = null,
            bool isDescending = false,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {
            Expression<Func<Product, object>>? expression = _getOrderExpression(orderBy)
                ?? throw new Exception("bad request");
            int? skip = _getSkip(page, limit);
            IEnumerable<Product> products = await _repository.OrderAndGet(
                expression,
                isDescending,
                skip,
                limit is null ? LIMIT : limit,
                isTracking,
                showDeleted,
                includes).ToListAsync();

            return _mapper.Map<ICollection<ProductGetItemDto>>(products);
        }
        public async Task<IEnumerable<ProductGetItemDto>> SearchProductsAsync(
            string searchTerm,
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {

            Expression<Func<Product, bool>> expression = c => c.Name.ToLower().Contains(searchTerm.ToLower());
            int? skip = _getSkip(page, limit);
            IEnumerable<Product> products = await _repository.SearchAndGet(
                expression,
                skip,
                limit is null ? LIMIT : limit,
                isTracking,
                showDeleted,
                includes).ToListAsync();

            return _mapper.Map<ICollection<ProductGetItemDto>>(products);

        }

        public async Task<ProductGetDto> GetProductByIdAsync(
            int id,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {
            Product product = await _repository.GetByIdAsync(id, isTracking, showDeleted, includes)
                ?? throw new Exception("Product wasnt found!");
            ProductGetDto dto = _mapper.Map<ProductGetDto>(product);
            foreach (ProductTag pt in product.ProductTags)
            {
                dto.Tags.Add(_mapper.Map<TagGetItemDto>(pt.Tag));
            }
            dto.Category = _mapper.Map<CategoryGetDto>(product.Category);
            return dto;


        }

        public async Task UpdateProductAsync(int id, ProductPutDto productDto)
        {
            string[] includes = { "ProductTags", "ProductTags.Tag" };
            Product product = await _repository.GetByIdAsync(id, isTracking:true, includes:includes) 
                ?? throw new Exception("Product wasnt found!");
            Category category = await _categoryRepository.GetByIdAsync(productDto.CategoryId)
                ?? throw new Exception($"Category with id {productDto.CategoryId} wasnt found!");


            if (productDto.TagIds is not null)
            {
                foreach (int tagId in productDto.TagIds)
                {
                    if (!product.ProductTags.Select(pt => pt.TagId).ToList().Contains(tagId))
                    {
                        Tag tag = await _tagRepository.GetByIdAsync(tagId)
                            ?? throw new Exception($"Tag with {tagId} wasnt found!");
                        product.ProductTags.Add(new ProductTag { TagId = tagId });
                    }
                }
                foreach (ProductTag pt in product.ProductTags)
                {
                    if (!productDto.TagIds.Contains(pt.TagId))
                    {
                        _productTagRepository.Delete(pt);
                    }
                }

            }
            else product.ProductTags = new List<ProductTag>();

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.CategoryId = product.CategoryId;

            await _repository.SaveChangesAsync();
            //ICollection<ProductTag> productTags = new List<ProductTag>();
            //if (productDto.TagIds is not null)
            //{
            //    foreach (int tagId in productDto.TagIds)
            //    {
            //        Tag tag = await _tagRepository.GetByIdAsync(tagId) 
            //            ?? throw new Exception($"Tag with {tagId} wasnt found!");
            //        productTags.Add(new ProductTag { TagId = tagId });
            //    }
            //}
            //product.ProductTags = productTags;

        }
        public async Task SoftDeleteProductAsync(int id)
        {
            Product product = await _repository.GetByIdAsync(id, true) ?? throw new Exception("Product wasnt found!");
            if (!product.IsDeleted)
            {
                product.IsDeleted = true;
                await _repository.SaveChangesAsync();
            }

        }
        public async Task RevertSoftDeleteProductAsync(int id)
        {
            Product product = await _repository.GetByIdAsync(id, true, true) ?? throw new Exception("Product wasnt found!");
            if (product.IsDeleted)
            {
                product.IsDeleted = false;
                await _repository.SaveChangesAsync();
            }

        }
        public async Task DeleteProductAsync(int id)
        {
            Product Product = await _repository.GetByIdAsync(id, showDeleted: true) ?? throw new Exception("Product wasnt found!");
            _repository.Delete(Product);
            await _repository.SaveChangesAsync();
        }


        // get expression for order method
        private Expression<Func<Product, object>> _getOrderExpression(string orderBy)
        {
            Expression<Func<Product, object>>? expression = null;
            switch (orderBy.ToLower())
            {
                case "name":
                    expression = c => c.Name;
                    break;
                case "id":
                    expression = c => c.Id;
                    break;
                case "price":
                    expression = c => c.Price;
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
