namespace TaskManagerApi.Services
{
    public interface IJwtService
    {
        string GenerateToken(string username);
    }
}