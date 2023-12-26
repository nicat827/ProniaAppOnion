using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProniaOnion.Application.Abstractions.Services;
using ProniaOnion.Application.Dtos;
using ProniaOnion.Application.Dtos.Color;

namespace ProniaOnion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IColorService _service;

        public ColorsController(IColorService service)
        {
            _service = service;
        }
        [HttpPost]

        public async Task<IActionResult> CreateAsync([FromForm] ColorPostDto colorDto)
        {

            return StatusCode(StatusCodes.Status201Created, await _service.CreateColorAsync(colorDto));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int? page = null, int? limit = null)
        {
            if (page < 1 || limit < 1) return NotFound();
            return Ok(await _service.GetColorsAsync(page, limit));
        }

        [HttpGet("/api/[controller]/order")]
        public async Task<IActionResult> GetAsync(string sort, bool desc = false, int? page = null, int? limit = null)
        {

            return Ok(await _service.GetOrderedColorsAsync(sort, page, limit, desc));
        }

        [HttpGet("/api/[controller]/search")]

        public async Task<IActionResult> GetAsync(string searchTerm, int? page = null, int? limit = null)
        {
            return Ok(await _service.SearchColorsAsync(searchTerm, page, limit));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            if (id <= 0) return BadRequest();

            return Ok(await _service.GetColorByIdAsync(id));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromForm] ColorPutDto colorDto)
        {
            if (id <= 0) return BadRequest();

            return Ok(await _service.UpdateColorAsync(id, colorDto));
        }



        [HttpPut("softDelete/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            if (id <= 0) return BadRequest();
            await _service.SoftDeleteColorAsync(id);
            return NoContent();
        }
        [HttpPut("revertSoftDelete/{id}")]
        public async Task<IActionResult> RevertSoftDelete(int id)
        {
            if (id <= 0) return BadRequest();
            await _service.RevertSoftDeleteColorAsync(id);
            return NoContent();
        }
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id <= 0) return NotFound();
            await _service.DeleteColorAsync(id);
            return NoContent();

        }
    }
}
