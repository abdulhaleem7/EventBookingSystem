using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Domain.Models.Contracts
{
    public abstract class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }

}
