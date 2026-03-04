using EventBookingSystem.Domain.Models;
using EventBookingSystem.Infrastructure.Data;
using EventBookingSystem.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBookingSystem.Infrastructure.Repositories.Implementations
{
    public class AdminRepository(AppDbContext context) : RepositoryAsync<Admin>(context), IAdminRepository
    {
    }
}
