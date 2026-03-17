using Livora_Lite.Application.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Livora_Lite.Application.Interface
{
    public interface IUserService
    {
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> CreateUserAsync(CreateUserRequestDTO request);
        Task<UserDTO> UpdateUserAsync(UpdateUserRequestDTO request);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ValidatePasswordAsync(string password, string hash);
    }
}
