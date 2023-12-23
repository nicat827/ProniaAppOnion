using ProniaOnion.Application.Dtos.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Abstractions.Services
{
    public interface ITagService
    {
        Task<TagGetDto> CreateTagAsync(TagCreateDto tagDto);

        Task DeleteTagAsync(int id);
        Task SoftDeleteTagAsync(int id);

        Task<TagGetDto> UpdateTagAsync(int id, TagUpdateDto tagDto);

        Task<ICollection<TagGetCollectionDto>> SearchTagsAsync(
            string searchTerm,
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            params string[] includes);

        Task<ICollection<TagGetCollectionDto>> GetOrderedTagsAsync(
           string orderBy,
           int? page = null,
           int? limit = null,
           bool isDescending = false,
           bool isTracking = false,
           params string[] includes);

        Task<TagGetDto> GetTagByIdAsync(int id, bool isTracking = false,bool showDeleted=false, params string[] includes);
        Task<ICollection<TagGetCollectionDto>> GetAllTagsAsync(int? page = null, int? limit = null, bool isTracking = false, bool showDeleted = false, params string[] includes);
    }
}
