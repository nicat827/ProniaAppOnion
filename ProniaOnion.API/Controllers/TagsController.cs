using Microsoft.AspNetCore.Mvc;
using ProniaOnion.Application.Abstractions.Services;
using ProniaOnion.Application.Dtos;

namespace ProniaOnion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _service;

        public TagsController(ITagService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] TagPostDto createTagDto)
        {

            return StatusCode(StatusCodes.Status201Created, await _service.CreateTagAsync(createTagDto));
        }
        [HttpGet]

        public async Task<IActionResult> Get(int? page = null, int? limit = null)
        {
            if (page < 1 || limit < 1) return NotFound();
            return Ok(await _service.GetTagsAsync(page, limit));
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



        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            if (id <= 0) return BadRequest();
            return Ok(await _service.GetTagByIdAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] TagPutDto tagDto)
        {
            if (id <= 0) return BadRequest();
            return Ok(await _service.UpdateTagAsync(id, tagDto));
        }

        [HttpPut("api/[controller]/softDelete/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            if (id <= 0) return BadRequest();
            await _service.SoftDeleteTagAsync(id);
            return NoContent();
        }

        [HttpPut("revertSoftDelete/{id}")]
        public async Task<IActionResult> RevertSoftDelete(int id)
        {
            if (id <= 0) return BadRequest();
            await _service.RevertSoftDeleteTagAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            
            await _service.DeleteTagAsync(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }


    }
}
