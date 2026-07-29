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
            if (login.Username == "admin" &&
                login.Password == "admin123")
            {
                var token = _jwtService.GenerateToken("admin", "Admin");

                return Ok(new
                {
                    Role = "Admin",
                    Token = token
                });
            }

            if (login.Username == "user" &&
                login.Password == "user123")
            {
                var token = _jwtService.GenerateToken("user", "User");

                return Ok(new
                {
                    Role = "User",
                    Token = token
                });
            }

            return Unauthorized("Invalid username or password.");
        }
    }
}