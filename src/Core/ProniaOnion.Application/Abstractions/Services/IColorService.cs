using ProniaOnion.Application.Dtos.Color;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Abstractions.Services
{
    public interface IColorService
    {
        Task<IEnumerable<ColorGetItemDto>> GetColorsAsync(
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);

        Task<IEnumerable<ColorGetItemDto>> GetOrderedColorsAsync(
            string orderBy,
            int? page = null,
            int? limit = null,
            bool isDescending = false,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);

        Task<ColorGetDto> GetColorByIdAsync(int id, bool isTracking = false, bool showDeleted = false, params string[] includes);

        Task<IEnumerable<ColorGetItemDto>> SearchColorsAsync(
            string searchTerm,
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);

        Task<ColorGetDto> CreateColorAsync(ColorPostDto ColorDto);



        Task<ColorGetDto> UpdateColorAsync(int id, ColorPutDto ColorDto);


        Task SoftDeleteColorAsync(int id);

        Task RevertSoftDeleteColorAsync(int id);
        Task DeleteColorAsync(int id);
    }
}
