using System;
using System.Collections.Generic;
using System.Text;

namespace EventBookingSystem.Domain.Enums
{
    public enum TransactionType
    {
        Credit = 1,
        Debit = 2
    }
    public enum TransactionStatus
    {
        Pending = 1,
        Completed = 2,
        Failed = 3
    }
}
