using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Domain.Models.Contracts
{
    public interface IAuditableEntity
    {
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
