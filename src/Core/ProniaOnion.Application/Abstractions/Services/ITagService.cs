using ProniaOnion.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Abstractions.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagGetItemDto>> GetTagsAsync(
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);

        Task<IEnumerable<TagGetItemDto>> GetOrderedTagsAsync(
            string orderBy,
            int? page = null,
            int? limit = null,
            bool isDescending = false,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);

        Task<TagGetDto> GetTagByIdAsync(int id, bool isTracking = false, bool showDeleted = false, params string[] includes);

        Task<IEnumerable<TagGetItemDto>> SearchTagsAsync(
            string searchTerm,
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);

        Task<TagGetDto> CreateTagAsync(TagPostDto tagDto);



        Task<TagGetDto> UpdateTagAsync(int id, TagPutDto tagDto);


        Task SoftDeleteTagAsync(int id);

        Task RevertSoftDeleteTagAsync(int id);
        Task DeleteTagAsync(int id);
    }
}
