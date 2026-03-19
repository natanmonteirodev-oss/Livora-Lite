using System.ComponentModel.DataAnnotations;
using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Application.DTO
{
    public class CreateMaintenanceRequestDTO
    {
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Imóvel é obrigatório")]
        public int PropertyId { get; set; }

        public int? ContractId { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data de abertura é obrigatória")]
        public DateTime RequestDate { get; set; }

        [Required(ErrorMessage = "Prioridade é obrigatória")]
        public MaintenancePriority Priority { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public MaintenanceStatus Status { get; set; }
    }

    public class UpdateMaintenanceRequestDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imóvel é obrigatório")]
        public int PropertyId { get; set; }

        public int? ContractId { get; set; }

        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data de abertura é obrigatória")]
        public DateTime RequestDate { get; set; }

        [Required(ErrorMessage = "Prioridade é obrigatória")]
        public MaintenancePriority Priority { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public MaintenanceStatus Status { get; set; }
    }
}