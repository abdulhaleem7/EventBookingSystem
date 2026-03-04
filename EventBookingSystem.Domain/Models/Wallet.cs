using EventBookingSystem.Domain.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Domain.Models
{
    public class Wallet : AuditableEntity
    {
        public string? UserId { get; set; }
        public User? User { get; set; }
        public decimal Balance { get; set; } = 0;
        public ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
    }
}
