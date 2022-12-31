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
                .OrderByDescending(p => p.IsDefault)
                .ToListAsync();
        }

        public async Task<Payment> GetPaymentByNumberAsync(string paymentNumber)
        {
            return await dbContext.Payments.FirstOrDefaultAsync(p => p.Number == paymentNumber);
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

            if (model.IsDefault)
            {
                if (defaultUserPayment != null)
                {
                    defaultUserPayment.IsDefault = false;
                }

                newUserPayment.IsDefault = true;
            }

            await dbContext.UserPayments.AddAsync(newUserPayment);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddUserPaymentAsync(string userId, string paymentId, bool isDefault)
        {
            UserPayment userPayment = new()
            {
                UserId = userId,
                PaymentId = paymentId,
            };

            await dbContext.UserPayments.AddAsync(userPayment);
            await dbContext.SaveChangesAsync();
            await SetDefaultPaymentAsync(userId, paymentId);
        }

        public async Task DeletePaymentAsync(string userId, string paymentId)
        {
            Payment? payment = await dbContext.Payments
                .FirstOrDefaultAsync(p => p.Id == paymentId && p.UserPayments
                                        .Select(up => up.UserId).Contains(userId));

            if (payment == null)
            {
                throw new InvalidOperationException("Извинявайте, нещо се обърка! Вашата карта не беше изтрита!");
            }

            dbContext.Payments.Remove(payment);
            await dbContext.SaveChangesAsync();
        }

        public async Task SetDefaultPaymentAsync(string userId, string paymentId)
        {
            UserPayment? newDefaultUserPayment = await dbContext.UserPayments
                .FirstOrDefaultAsync(up => up.UserId == userId && up.PaymentId == paymentId);

            UserPayment? oldDefaultUserPayment = await GetDefaultUserPaymentAsync(userId);

            if (oldDefaultUserPayment != null)
            {
                oldDefaultUserPayment.IsDefault = false;
            }

            if (newDefaultUserPayment != null)
            {
                newDefaultUserPayment.IsDefault = true;
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<BankCardViewModel> GetPaymentForUserAsync(string userId, string paymentId)
        {
            BankCardViewModel? payment = await dbContext.Payments
                .Where(p => p.Id == paymentId && p.UserPayments
                            .Select(up => up.UserId).Contains(userId))
                .Select(p => new BankCardViewModel()
                {
                    Amount = p.Amount,
                    CardNumber = p.Number,
                    CVC = p.CVC,
                    ExpireDate = p.ExpireDate,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    IsDefault = p.UserPayments.FirstOrDefault(up => up.UserId == userId).IsDefault
                })
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                throw new ArgumentException("Нещо се обърка!");
            }

            return payment;
        }
    }
}