using Microsoft.EntityFrameworkCore;
using UrbanLife.Core.ViewModels;
using UrbanLife.Data.Data;
using UrbanLife.Data.Data.Models;
using UrbanLife.Data.Enums;

namespace UrbanLife.Core.Services
{
    public class PaymentService
    {
        private readonly ScheduleService scheduleService;
        private readonly ApplicationDbContext dbContext;

        public PaymentService(ScheduleService scheduleService, ApplicationDbContext dbContext)
        {
            this.scheduleService = scheduleService;
            this.dbContext = dbContext;
        }

        public async Task<List<Payment>> GetPaymentsByUserAsync(string userId)
        {
            return await dbContext.Payments
                .Where(p => p.UserPayments.Select(up => up.UserId).Contains(userId))
                .ToListAsync();
        }

        public async Task<bool> CheckIfPaymentIsDefaultAsync(string userId, string cardNumber)
        {
            return await dbContext.UserPayments
                .AnyAsync(up => up.UserId == userId && up.Payment.Number == cardNumber && up.IsDefault);
        }

        public async Task<List<SubscriptionPaymentViewModel>> GetSubscriptionPaymentsForUserAsync(string userId)
        {
            return await dbContext.UserPayments
                .Where(up => up.UserId == userId)
                .OrderByDescending(up => up.IsDefault)
                .Select(up => new SubscriptionPaymentViewModel
                {
                    CardNumber = up.Payment.Number,
                    IsDefault = up.IsDefault
                })
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
                .ThenByDescending(p => p.Amount)
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

            if (isDefault)
            {
                await SetDefaultPaymentAsync(userId, paymentId);
            }
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

        public async Task PurchaseSubscriptionAsync(BuySubscriptionViewModel model,
            Purchase purchase, string webHostEnvironmentUrl)
        {
            purchase.Amount = model.FinalPrice;
            purchase.Type = model.SubscriptionType;
            purchase.Date = DateTime.Now;

            Payment payment = await GetPaymentByNumberAsync(model.ChosenCardNumber);
            purchase.PaymentId = payment.Id;

            if (model.ChosenTicketStartTime.HasValue)
            {
                DateTime ticketStartDateTime = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month,
                    day: DateTime.Now.Day, hour: model.ChosenTicketStartTime.Value.Hours,
                    minute: model.ChosenTicketStartTime.Value.Minutes, second: model.ChosenTicketStartTime.Value.Seconds);

                purchase.Start = ticketStartDateTime;

                if (model.ChosenDuration == "one-way" || model.ChosenDuration == "60-minute")
                {
                    purchase.End = ticketStartDateTime.AddHours(1);
                }
                else if (model.ChosenDuration == "30-minute")
                {
                    purchase.End = ticketStartDateTime.AddMinutes(30);
                }
                else if (model.ChosenDuration == "1-day")
                {
                    purchase.End = ticketStartDateTime.AddDays(1);
                }
            }
            else if (model.ChosenCardStartDate.HasValue)
            {
                purchase.Start = model.ChosenCardStartDate.Value;

                if (model.ChosenDuration == "1-month")
                {
                    purchase.End = model.ChosenCardStartDate.Value.AddMonths(1);
                }
                else if (model.ChosenDuration == "3-month")
                {
                    purchase.End = model.ChosenCardStartDate.Value.AddMonths(3);
                }
                else if (model.ChosenDuration == "1-year")
                {
                    purchase.End = model.ChosenCardStartDate.Value.AddYears(1);
                }
            }

            await ExecuteTransactionAsync(payment.Id, model.FinalPrice);
            await dbContext.Purchases.AddAsync(purchase);
            await dbContext.SaveChangesAsync();

            string[] chosenLinesGroups = model.ChosenLines.Split(',');

            if (!chosenLinesGroups.Contains("all-lines"))
            {
                List<PurchaseLine> purchaseLines = new();

                foreach (string lineGroup in chosenLinesGroups)
                {
                    string[] parsedLineTokens = lineGroup.Split('-');
                    int lineNumber = int.Parse(parsedLineTokens[0]);
                    LineType lineType = (LineType)Enum.Parse(typeof(LineType), parsedLineTokens[1], true);

                    Line line = await scheduleService.GetLineIdAsync(lineNumber, lineType);
                    purchaseLines.Add(new PurchaseLine { LineId = line.Id, PurchaseId = purchase.Id });
                }

                await dbContext.PurchaseLines.AddRangeAsync(purchaseLines);
                await dbContext.SaveChangesAsync();
            }

        }

        public async Task ExecuteTransactionAsync(string paymentId, decimal price)
        {
            Payment? payment = await dbContext.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);

            if (payment != null && payment.Amount >= price)
            {
                payment.Amount -= price;
            }

            await dbContext.SaveChangesAsync();
        }
    }
}