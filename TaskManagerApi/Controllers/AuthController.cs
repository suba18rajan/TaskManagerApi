using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.DTOs;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO login)
        {
            // Temporary hardcoded user
            if (login.Username != "admin" || login.Password != "admin123")
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = _jwtService.GenerateToken(login.Username);

            return Ok(new
            {
                Token = token
            });
        }
    }
}