using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProniaOnion.Application.Abstractions.Services;
using ProniaOnion.Application.Dtos;

namespace ProniaOnion.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class ProductsController: ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] ProductPostDto productDto)
        {
            await _service.CreateProductAsync(productDto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet] 
        public async Task<IActionResult> GetAll(int? page = null , int? limit = null)
        {
            if (page < 1 || limit < 1) return NotFound();
            return Ok(await _service.GetProductsAsync(page, limit));
        }
        [HttpGet("search")]

        public async Task<IActionResult> GetAsync(string searchTerm, int? page = null, int? limit = null)
        {
            return Ok(await _service.SearchProductsAsync(searchTerm, page, limit));
        }
        [HttpGet("order")]
        public async Task<IActionResult> GetAsync(string sort, bool desc = false, int? page = null, int? limit = null)
        {

            return Ok(await _service.GetOrderedProductsAsync(sort, page, limit, desc));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            if (id <= 0) return BadRequest();
            string[] includes = { "ProductTags", "ProductTags.Tag", "Category" };
            return Ok(await _service.GetProductByIdAsync(id, includes:includes));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] ProductPutDto productDto)
        {
            if (id <= 0) return BadRequest();
            await _service.UpdateProductAsync(id, productDto);
            return Ok();
        }
        [HttpPut("softDelete/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            if (id <= 0) return BadRequest();
            await _service.SoftDeleteProductAsync(id);
            return NoContent();
        }
        [HttpPut("revertSoftDelete/{id}")]
        public async Task<IActionResult> RevertSoftDelete(int id)
        {
            if (id <= 0) return BadRequest();
            await _service.RevertSoftDeleteProductAsync(id);
            return NoContent();
        }
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id <= 0) return NotFound();
            await _service.DeleteProductAsync(id);
            return NoContent();

        }

    }
}
