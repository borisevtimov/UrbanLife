using Microsoft.EntityFrameworkCore;
using UrbanLife.Core.ViewModels;
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

        public async Task<List<Payment>> GetPaymentsByUserAsync(string userId)
        {
            return await dbContext.Payments
                .Where(p => p.UserPayments.Select(up => up.UserId).Contains(userId))
                .ToListAsync();
        }

        public async Task<UserPayment> GetDefaultUserPaymentAsync(string userId)
        {
            return await dbContext.UserPayments
                .FirstOrDefaultAsync(up => up.UserId == userId && up.IsDefault);
        }

        public async Task<List<PaymentMethodViewModel>> GetPaymentsForAccountPageAsync(string userId)
        {
            return await dbContext.Payments
                .Where(p => p.UserPayments.Select(up => up.UserId).Contains(userId))
                .Select(p => new PaymentMethodViewModel
                {
                    PaymentId = p.Id,
                    CardNumber = p.Number,
                    CardFirstName = p.FirstName,
                    CardLastName = p.LastName,
                    Amount = p.Amount,
                    CVC = p.CVC,
                    ExpireDate = p.ExpireDate,
                    IsDefault = p.UserPayments.Select(up => up.IsDefault == true).FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task AddPaymentMethodAsync(string userId, BankCardViewModel model)
        {
            Payment payment = new()
            {
                FirstName = model.FirstName.ToUpper(),
                LastName = model.LastName.ToUpper(),
                Number = model.CardNumber,
                CVC = model.CVC,
                Amount = model.Amount.GetValueOrDefault(),
                ExpireDate = model.ExpireDate,
            };

            await dbContext.Payments.AddAsync(payment);
            await dbContext.SaveChangesAsync();

            UserPayment newUserPayment = new()
            {
                PaymentId = payment.Id,
                UserId = userId
            };

            UserPayment defaultUserPayment = await GetDefaultUserPaymentAsync(userId);

            if (defaultUserPayment != null && model.IsDefault)
            {
                defaultUserPayment.IsDefault = false;
                newUserPayment.IsDefault = true;
            }

            await dbContext.UserPayments.AddAsync(newUserPayment);
            await dbContext.SaveChangesAsync();
        }
    }
}