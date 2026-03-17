namespace Livora_Lite.Application.DTO
{
    public class UpdateTenantRequestDTO
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Primeiro nome é obrigatório")]
        [System.ComponentModel.DataAnnotations.StringLength(50, ErrorMessage = "Primeiro nome não pode ter mais de 50 caracteres")]
        public string FirstName { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Sobrenome é obrigatório")]
        [System.ComponentModel.DataAnnotations.StringLength(50, ErrorMessage = "Sobrenome não pode ter mais de 50 caracteres")]
        public string LastName { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "CPF ou CNPJ é obrigatório")]
        [System.ComponentModel.DataAnnotations.StringLength(20, ErrorMessage = "Documento não pode ter mais de 20 caracteres")]
        public string Document { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Telefone é obrigatório")]
        [System.ComponentModel.DataAnnotations.StringLength(20, ErrorMessage = "Telefone não pode ter mais de 20 caracteres")]
        public string Phone { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Email é obrigatório")]
        [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Endereço é obrigatório")]
        [System.ComponentModel.DataAnnotations.StringLength(500, ErrorMessage = "Endereço não pode ter mais de 500 caracteres")]
        public string CurrentAddress { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue, ErrorMessage = "Por favor, selecione um status válido")]
        public int TenantStatusId { get; set; }
    }
}