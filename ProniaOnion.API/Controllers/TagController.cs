using Microsoft.AspNetCore.Mvc;
using ProniaOnion.Application.Abstractions.Services;
using ProniaOnion.Application.Dtos.Tag;

namespace ProniaOnion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _service;

        public TagController(ITagService service)
        {
            _service = service;
        }

        [HttpGet]

        public async Task<IActionResult> Get(int? page = null, int? limit = null)
        {
            if (page < 1 || limit < 1) return NotFound();
            return Ok(await _service.GetAllTagsAsync(page, limit));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            if (id <= 0) return BadRequest();
            return Ok(await _service.GetTagByIdAsync(id));
        }
        [HttpGet("/api/[controller]/search")]

        public async Task<IActionResult> GetAsync(string searchTerm, int? page = null, int? limit = null)
        {
            return Ok(await _service.SearchTagsAsync(searchTerm, page, limit));
        }

        [HttpGet("/api/[controller]/order")]
        public async Task<IActionResult> GetAsync(string sort, bool desc = false, int? page = null, int? limit = null)
        {

            return Ok(await _service.GetOrderedTagsAsync(sort, page, limit, desc));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] TagCreateDto createTagDto)
        {

            return StatusCode(StatusCodes.Status201Created, await _service.CreateTagAsync(createTagDto));
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            
            await _service.DeleteTagAsync(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPut("api/[controller]/softDelete/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            if (id <= 0) return BadRequest();
            await _service.SoftDeleteTagAsync(id);
            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> Put(int id, [FromForm] TagUpdateDto tagDto)
        {
            if (id <= 0) return BadRequest();
            return Ok(await _service.UpdateTagAsync(id, tagDto));
        }

    }
}
