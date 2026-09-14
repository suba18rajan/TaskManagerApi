using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.DTOs;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var result =
                await _authService.LoginAsync(login);

            if (result == null)
            {
                return Unauthorized(
                    "Invalid username or password.");
            }

            return Ok(new
            {
                Role = result.Value.Role,
                Token = result.Value.Token
            });
        }
    }
}