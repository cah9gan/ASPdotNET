using TransportStore.BlazorClient.Models;

namespace TransportStore.BlazorClient.Services
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterDto registerModel);
        Task<AuthResponseDto?> Login(LoginDto loginModel);
        Task Logout();
    }
}