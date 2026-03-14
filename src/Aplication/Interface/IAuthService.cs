using Livora_Lite.Application.DTO;

namespace Livora_Lite.Application.Interface
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request);
        Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO request);
        Task<UserDTO?> GetUserByEmailAsync(string email);
        Task<bool> ValidatePasswordAsync(string password, string hash);
    }
}
