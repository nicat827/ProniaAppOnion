using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstractions.Repositories;
using ProniaOnion.Application.Abstractions.Services;
using ProniaOnion.Application.Dtos.Tag;
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
        private readonly ITagRepository _repository;
        private readonly IMapper _mapper;

        public TagService(ITagRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ICollection<TagGetCollectionDto>> GetAllTagsAsync(int? page = null, int? limit = null, bool isTracking = false, bool showDeleted = false, params string[] includes)
        {
            ICollection<Tag> tags = new List<Tag>();
            if (page == null)
            {
                if (limit == null) tags = await _repository.GetAll(isTracking: isTracking, includes: includes).ToListAsync();
                else tags = await _repository.GetAll(limit: (int)limit, isTracking: isTracking, includes: includes).ToListAsync();
            }
            else
            {
                limit ??= 5;
                tags = await _repository.GetAll((int)((page - 1) * limit), (int)limit, isTracking,false, includes).ToListAsync();
            }

            return _mapper.Map<ICollection<TagGetCollectionDto>>(tags);


        }

        public async Task<TagGetDto> GetTagByIdAsync(int id, bool isTracking = false,bool showDeleted=false, params string[] includes)
        {
            Tag tag = await _repository.GetByIdAsync(id, isTracking,showDeleted, includes) ?? throw new Exception("Tag wasnt found!");
            return _mapper.Map<TagGetDto>(tag);


        }



        public async Task<TagGetDto> CreateTagAsync(TagCreateDto tagDto)
        {
            Tag tag = new Tag { Name = tagDto.Name };
            await _repository.CreateAsync(tag);
            await _repository.SaveChangesAsync();
            return _mapper.Map<TagGetDto>(tag);
        }

        public async Task DeleteTagAsync(int id)
        {
            Tag tag = await _repository.GetByIdAsync(id, showDeleted:true) ?? throw new Exception("Tag wasnt found!");
            _repository.Delete(tag);
            await _repository.SaveChangesAsync();
        }

        public async Task<TagGetDto> UpdateTagAsync(int id, TagUpdateDto tagDto)
        {
            Tag tag = await _repository.GetByIdAsync(id:id, showDeleted:true) ?? throw new Exception("Tag wasnt found!");
            tag.Name = tagDto.Name;
            _repository.Update(tag);
            await _repository.SaveChangesAsync();
            return _mapper.Map<TagGetDto>(tag);
        }
        public async Task<ICollection<TagGetCollectionDto>> GetOrderedTagsAsync(
           string orderBy,
           int? page = null,
           int? limit = null,
           bool isDescending = false,
           bool isTracking = false,
           params string[] includes)
        {
            Expression<Func<Tag, object>>? expression = GetOrderExpression(orderBy)
                ?? throw new Exception("bad request");
            ICollection<Tag> tags = new List<Tag>();

            if (page == null)
            {
                if (limit == null) tags = await _repository.OrderAndGet(order: expression, isDescending: isDescending, isTracking: isTracking, includes: includes).ToListAsync();
                else tags = await _repository.OrderAndGet(order: expression, isDescending: isDescending, limit: (int)limit, isTracking: isTracking, includes: includes).ToListAsync();
            }
            else
            {
                if (limit == null) limit = 5;
                tags = await _repository.OrderAndGet(order: expression, isDescending: isDescending, skip: (int)((page - 1) * limit), (int)limit, isTracking,false, includes).ToListAsync();

            }

            return _mapper.Map<ICollection<TagGetCollectionDto>>(tags);
        }

        public async Task<ICollection<TagGetCollectionDto>> SearchTagsAsync(
            string searchTerm,
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            params string[] includes)
        {
            ICollection<Tag> tags = new List<Tag>();
            Expression<Func<Tag, bool>> expression = t => t.Name.ToLower().Contains(searchTerm.ToLower());
            if (page == null)
            {
                if (limit == null) tags = await _repository.SearchAndGet(expression: expression, isTracking: isTracking, includes: includes).ToListAsync();
                else tags = await _repository.SearchAndGet(expression: expression, limit: (int)limit, isTracking: isTracking, includes: includes).ToListAsync();
            }
            else
            {
                limit ??= 5;
                tags = await _repository.SearchAndGet(expression: expression, skip: (int)((page - 1) * limit), (int)limit, isTracking, includes).ToListAsync();

            }

            return _mapper.Map<ICollection<TagGetCollectionDto>>(tags);
        }

        // get expression for order method
        public Expression<Func<Tag, object>> GetOrderExpression(string orderBy)
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

        public async Task SoftDeleteTagAsync(int id)
        {
            Tag tag = await _repository.GetByIdAsync(id) ?? throw new Exception("Tag wasnt found!");
            if (!tag.IsDeleted)
            {
                tag.IsDeleted = true;
                _repository.SoftDelete(tag);
                await _repository.SaveChangesAsync();
            }
            
        }
    }
}
