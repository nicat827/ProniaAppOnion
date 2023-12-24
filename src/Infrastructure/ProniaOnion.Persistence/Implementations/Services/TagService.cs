using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    internal class TagService : ITagService
    {
        private const int LIMIT = 5;
        private readonly ITagRepository _repository;
        private readonly IMapper _mapper;

        public TagService(ITagRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<TagGetDto> CreateTagAsync(TagPostDto tagDto)
        {

            Tag newTag = _mapper.Map<Tag>(tagDto);
            await _repository.CreateAsync(newTag);
            await _repository.SaveChangesAsync();
            return _mapper.Map<TagGetDto>(newTag);
        }


        public async Task<IEnumerable<TagGetItemDto>> GetTagsAsync(
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes
        )
        {
            int? skip = _getSkip(page, limit);
            IEnumerable<Tag> tags = await _repository.GetAll(
                 skip,
                 limit is null && page is not null ? LIMIT : limit,
                 isTracking,
                 showDeleted,
                 includes).ToListAsync();

            return _mapper.Map<IEnumerable<TagGetItemDto>>(tags);
        }


        public async Task<IEnumerable<TagGetItemDto>> GetOrderedTagsAsync(
            string orderBy,
            int? page = null,
            int? limit = null,
            bool isDescending = false,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {
            Expression<Func<Tag, object>>? expression = _getOrderExpression(orderBy)
                ?? throw new Exception("bad request");
            int? skip = _getSkip(page, limit);
            IEnumerable<Tag> tags = await _repository.OrderAndGet(
                expression,
                isDescending,
                skip,
                limit is null && page is not null ? LIMIT : limit,
                isTracking,
                showDeleted,
                includes).ToListAsync();

            return _mapper.Map<ICollection<TagGetItemDto>>(tags);
        }
        public async Task<IEnumerable<TagGetItemDto>> SearchTagsAsync(
            string searchTerm,
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {

            Expression<Func<Tag, bool>> expression = c => c.Name.ToLower().Contains(searchTerm.ToLower());
            int? skip = _getSkip(page, limit);
            IEnumerable<Tag> tags = await _repository.SearchAndGet(
                expression,
                skip,
                limit is null && page is not null ? LIMIT : limit,
                isTracking,
                showDeleted,
                includes).ToListAsync();

            return _mapper.Map<ICollection<TagGetItemDto>>(tags);

        }

        public async Task<TagGetDto> GetTagByIdAsync(
            int id,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {
            Tag Tag = await _repository.GetByIdAsync(id, isTracking, showDeleted, includes)
                ?? throw new Exception("Tag wasnt found!");
            return _mapper.Map<TagGetDto>(Tag);


        }

        public async Task<TagGetDto> UpdateTagAsync(int id, TagPutDto TagDto)
        {
            Tag Tag = await _repository.GetByIdAsync(id, true) ?? throw new Exception("Tag wasnt found!");
            Tag.Name = TagDto.Name;
            await _repository.SaveChangesAsync();
            return _mapper.Map<TagGetDto>(Tag);

        }
        public async Task SoftDeleteTagAsync(int id)
        {
            Tag tag = await _repository.GetByIdAsync(id, true) ?? throw new Exception("Tag wasnt found!");
            if (!tag.IsDeleted)
            {
                _repository.SoftDelete(tag);
                await _repository.SaveChangesAsync();
            }

        }

        public async Task RevertSoftDeleteTagAsync(int id)
        {
            Tag tag = await _repository.GetByIdAsync(id, true, true) ?? throw new Exception("Tag wasnt found!");
            if (!tag.IsDeleted)
            {
                _repository.RevertSoftDelete(tag);
                await _repository.SaveChangesAsync();
            }

        }
        public async Task DeleteTagAsync(int id)
        {
            Tag Tag = await _repository.GetByIdAsync(id, showDeleted: true) ?? throw new Exception("Tag wasnt found!");
            _repository.Delete(Tag);
            await _repository.SaveChangesAsync();
        }


        // get expression for order method
        private Expression<Func<Tag, object>> _getOrderExpression(string orderBy)
        {
            Expression<Func<Tag, object>>? expression = null;
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
