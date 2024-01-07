using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProniaOnion.Application.Abstractions.Services;
using ProniaOnion.Application.Dtos.AppUser;

namespace ProniaOnion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _service;

        public UsersController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Post([FromForm]RegisterDto registerDto)
        {
            await _service.Register(registerDto);
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpGet("confirm")]

        public async Task<IActionResult> Get(string token, string email)
        {
            await _service.ConfirmEmailAsync(token, email);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromForm]LoginDto loginDto)
        {
            
            return Ok(await _service.LoginAsync(loginDto));
        }
        
        [HttpPost("refresh")]
        public async Task<IActionResult> Post([FromForm] string refreshToken)
        {

            return Ok(await _service.RefreshTokensAsync(refreshToken));
        }

        [HttpGet]

        public async Task<IActionResult> Get(int? page = null, int? limit = null)
        {

            if (page < 1 || limit < 1) return NotFound();
            return Ok(await _service.GetAllUsers(page, limit));
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _service.GetUserByIdAsync(id));
        }

    }
}
