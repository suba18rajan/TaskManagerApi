using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;

namespace TaskManagerApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;

        public AuthService(
            AppDbContext context,
            IPasswordHasher passwordHasher,
            IJwtService jwtService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<(string Token, string Role)?> LoginAsync(
            LoginDTO login)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Username == login.Username);

            if (user == null)
                return null;

            bool passwordValid =
                _passwordHasher.Verify(
                    login.Password,
                    user.PasswordHash);

            if (!passwordValid)
                return null;

            string token =
                _jwtService.GenerateToken(
                    user.Username,
                    user.Role);

            return (token, user.Role);
        }
    }
}