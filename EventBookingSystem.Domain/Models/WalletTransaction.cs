using EventBookingSystem.Domain.Enums;
using EventBookingSystem.Domain.Models.Contracts;

namespace EventBookingSystem.Domain.Models
{
    public class WalletTransaction : AuditableEntity
    {

        public string? WalletId { get; set; }
        public Wallet? Wallet { get; set; }

        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }

        public string? Reference { get; set; }

        public TransactionStatus Status { get; set; }

        public string? IdempotencyKey { get; set; }
    }
}
