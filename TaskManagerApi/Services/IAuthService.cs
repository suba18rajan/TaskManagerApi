using TaskManagerApi.DTOs;

namespace TaskManagerApi.Services
{
    public interface IAuthService
    {
        Task<(string Token, string Role)?> LoginAsync(LoginDTO login);
    }
}