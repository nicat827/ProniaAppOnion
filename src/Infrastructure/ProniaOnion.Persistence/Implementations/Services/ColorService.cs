using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstractions.Repositories;
using ProniaOnion.Application.Abstractions.Services;
using ProniaOnion.Application.Dtos.Color;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Persistence.Implementations.Services
{
    internal class ColorService:IColorService
    {
        private const int LIMIT = 5;
        private readonly IColorRepository _repository;
        private readonly IMapper _mapper;

        public ColorService(IColorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ColorGetDto> CreateColorAsync(ColorPostDto tagDto)
        {

            Color newColor = _mapper.Map<Color>(tagDto);
            await _repository.CreateAsync(newColor);
            await _repository.SaveChangesAsync();
            return _mapper.Map<ColorGetDto>(newColor);
        }


        public async Task<IEnumerable<ColorGetItemDto>> GetColorsAsync(
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes
        )
        {
            int? skip = _getSkip(page, limit);
            IEnumerable<Color> colors = await _repository.GetAll(
                 skip,
                 limit is null && page is not null ? LIMIT : limit,
                 isTracking,
                 showDeleted,
                 includes).ToListAsync();

            return _mapper.Map<IEnumerable<ColorGetItemDto>>(colors);
        }


        public async Task<IEnumerable<ColorGetItemDto>> GetOrderedColorsAsync(
            string orderBy,
            int? page = null,
            int? limit = null,
            bool isDescending = false,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {
            Expression<Func<Color, object>>? expression = _getOrderExpression(orderBy)
                ?? throw new Exception("bad request");
            int? skip = _getSkip(page, limit);
            IEnumerable<Color> colors = await _repository.OrderAndGet(
                expression,
                isDescending,
                skip,
                limit is null && page is not null ? LIMIT : limit,
                isTracking,
                showDeleted,
                includes).ToListAsync();

            return _mapper.Map<ICollection<ColorGetItemDto>>(colors);
        }
        public async Task<IEnumerable<ColorGetItemDto>> SearchColorsAsync(
            string searchTerm,
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {

            Expression<Func<Color, bool>> expression = c => c.Name.ToLower().Contains(searchTerm.ToLower());
            int? skip = _getSkip(page, limit);
            IEnumerable<Color> colors = await _repository.SearchAndGet(
                expression,
                skip,
                limit is null && page is not null ? LIMIT : limit,
                isTracking,
                showDeleted,
                includes).ToListAsync();

            return _mapper.Map<ICollection<ColorGetItemDto>>(colors);

        }

        public async Task<ColorGetDto> GetColorByIdAsync(
            int id,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {
            Color color = await _repository.GetByIdAsync(id, isTracking, showDeleted, includes)
                ?? throw new Exception("Color wasnt found!");
            return _mapper.Map<ColorGetDto>(color);


        }

        public async Task<ColorGetDto> UpdateColorAsync(int id, ColorPutDto ColorDto)
        {
            Color Color = await _repository.GetByIdAsync(id, true) ?? throw new Exception("Color wasnt found!");
            Color.Name = ColorDto.Name;
            await _repository.SaveChangesAsync();
            return _mapper.Map<ColorGetDto>(Color);

        }
        public async Task SoftDeleteColorAsync(int id)
        {
            Color tag = await _repository.GetByIdAsync(id, true) ?? throw new Exception("Color wasnt found!");
            if (!tag.IsDeleted)
            {
                _repository.SoftDelete(tag);
                await _repository.SaveChangesAsync();
            }

        }

        public async Task RevertSoftDeleteColorAsync(int id)
        {
            Color color = await _repository.GetByIdAsync(id, true, true) ?? throw new Exception("Color wasnt found!");
            if (!color.IsDeleted)
            {
                _repository.RevertSoftDelete(color);
                await _repository.SaveChangesAsync();
            }

        }
        public async Task DeleteColorAsync(int id)
        {
            Color color = await _repository.GetByIdAsync(id, showDeleted: true) ?? throw new Exception("Color wasnt found!");
            _repository.Delete(color);
            await _repository.SaveChangesAsync();
        }


        // get expression for order method
        private Expression<Func<Color, object>> _getOrderExpression(string orderBy)
        {
            Expression<Func<Color, object>>? expression = null;
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
