using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProniaOnion.Application.Abstractions.Services;
using ProniaOnion.Application.Dtos.Category;

namespace ProniaOnion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int? page = null, int? limit = null)
        {
            if (page < 1 || limit < 1) return NotFound();
            return Ok(await _service.GetCategoriesAsync(page, limit));
        }

        [HttpGet("/api/[controller]/order")]
        public async Task<IActionResult> GetAsync(string sort, bool desc = false, int? page = null, int? limit = null)
        {

            return Ok(await _service.GetOrderedCategoriesAsync(sort, page, limit, desc));
        }

        [HttpGet("/api/[controller]/search")]

        public async Task<IActionResult> GetAsync(string searchTerm, int? page = null, int? limit = null)
        {
            return Ok(await _service.SearchCategoriesAsync(searchTerm, page, limit));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            if (id <= 0) return BadRequest();

            return Ok(await _service.GetCategoryByIdAsync(id));
        }

        [HttpPost]

        public async Task<IActionResult> CreateAsync([FromForm] CategoryCreateDto categoryDto)
        {

            return StatusCode(StatusCodes.Status201Created, await _service.CreateCategoryAsync(categoryDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id,[FromForm]CategoryUpdateDto categoryDto)
        {
            if (id <= 0) return BadRequest();

            return Ok(await _service.UpdateCategoryAsync(id, categoryDto));
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id <= 0) return NotFound();
            await _service.DeleteCategory(id);
            return NoContent();

        }

        [HttpPut("api/[controller]/softDelete/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            if (id <= 0) return BadRequest();
            await _service.SoftDeleteCategoryAsync(id);
            return NoContent();
        }

    }
}
