using Microsoft.EntityFrameworkCore;
using UrbanLife.Data.Data;
using UrbanLife.Data.Data.Models;

namespace UrbanLife.Core.Services
{
    public class PaymentService
    {
        private readonly ApplicationDbContext dbContext;

        public PaymentService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Payment>> GetPaymentsByUser(string userId)
        {
            return await dbContext.Payments
                .Where(p => p.UserPayments.Select(up => up.UserId).Contains(userId))
                .ToListAsync();
        }

        public async Task<string> GetDefaultPayment(string userId)
        {
            UserPayment? userPayment = await dbContext.UserPayments
                .FirstOrDefaultAsync(up => up.UserId == userId && up.IsDefault);

            return userPayment == null ? null : userPayment.PaymentNumber;
        }
    }
}