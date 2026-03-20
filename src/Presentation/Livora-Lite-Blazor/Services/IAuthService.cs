using Livora_Lite.Application.DTO;
using Microsoft.AspNetCore.Components;

namespace Livora_Lite_Blazor.Services
{
    /// <summary>
    /// Interface para serviço de autenticação
    /// Define os métodos disponíveis para autenticação de usuários
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Realiza o login de um usuário
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <param name="password">Senha do usuário</param>
        /// <returns>Resposta com token e dados do usuário</returns>
        Task<AuthResponseDTO> LoginAsync(string email, string password);

        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        /// <param name="firstName">Primeiro nome do usuário</param>
        /// <param name="lastName">Sobrenome do usuário</param>
        /// <param name="email">Email do usuário</param>
        /// <param name="password">Senha do usuário</param>
        /// <param name="confirmPassword">Confirmação de senha</param>
        /// <returns>Resposta com token e dados do novo usuário</returns>
        Task<AuthResponseDTO> RegisterAsync(string firstName, string lastName, string email, string password, string confirmPassword);

        /// <summary>
        /// Faz logout do usuário
        /// </summary>
        /// <returns>Task de conclusão</returns>
        Task LogoutAsync();

        /// <summary>
        /// Verifica se o usuário está autenticado
        /// </summary>
        /// <returns>True se autenticado, false caso contrário</returns>
        bool IsAuthenticated();

        /// <summary>
        /// Obtém o token JWT armazenado
        /// </summary>
        /// <returns>Token JWT ou null</returns>
        string? GetToken();

        /// <summary>
        /// Obtém o usuário autenticado
        /// </summary>
        /// <returns>UserDTO do usuário autenticado ou null</returns>
        UserDTO? GetCurrentUser();

        /// <summary>
        /// Atualiza o usuário atual
        /// </summary>
        /// <param name="user">Dados do usuário</param>
        void SetCurrentUser(UserDTO? user);

        /// <summary>
        /// Atualiza o token armazenado
        /// </summary>
        /// <param name="token">Novo token</param>
        void SetToken(string? token);
    }
}
