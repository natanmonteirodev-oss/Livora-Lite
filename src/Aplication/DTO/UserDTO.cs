using System.ComponentModel.DataAnnotations;

namespace Livora_Lite.Application.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "Home";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();
    }

    public class CreateUserRequestDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sobrenome é obrigatório")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role é obrigatória")]
        public string Role { get; set; } = "Home";
    }

    public class UpdateUserRequestDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sobrenome é obrigatório")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role é obrigatória")]
        public string Role { get; set; } = "Home";

        public bool IsActive { get; set; } = true;
    }
}
