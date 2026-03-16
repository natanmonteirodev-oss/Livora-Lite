using System;

namespace Livora_Lite.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string? UserId { get; set; } // ID do usuário que realizou a ação
        public string? UserName { get; set; } // Nome do usuário para facilitar consultas
        public string? Action { get; set; } // Ex: "Criado", "Alterado", "Excluído"
        public string? Entity { get; set; } // Ex: "Property", "Contract", "Payment"
        public string? EntityId { get; set; } // ID da entidade afetada
        public DateTime Date { get; set; } // Data e hora da ação
        public string? Changes { get; set; } // Descrição das alterações (JSON ou texto)
        public string? OldValues { get; set; } // Valores antigos (opcional, JSON)
        public string? NewValues { get; set; } // Novos valores (opcional, JSON)
    }
}