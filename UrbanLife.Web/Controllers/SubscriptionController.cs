using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbanLife.Core.Services;
using UrbanLife.Core.Utilities;
using UrbanLife.Core.Utilities.Constants;
using UrbanLife.Core.ViewModels;
using UrbanLife.Data.Data.Models;
using UrbanLife.Data.Enums;

namespace UrbanLife.Web.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly PaymentService paymentService;
        private readonly ScheduleService scheduleService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public SubscriptionController(PaymentService paymentService,
            UserManager<User> userManager,
            ScheduleService scheduleService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.paymentService = paymentService;
            this.userManager = userManager;
            this.scheduleService = scheduleService;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult All()
        {
            return View();
        }

        public async Task<IActionResult> Purchase(SubscriptionType subscriptionType, int? lineNumber,
            LineType? lineType, TimeSpan? ticketStartTime)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/user/login");
            }

            UrbanLife.Data.Data.Models.User user = await userManager.GetUserAsync(User);

            BuySubscriptionViewModel model = new()
            {
                SubscriptionType = subscriptionType,
                LineType = lineType,
                LineNumber = lineNumber,
                ChosenTicketStartTime = ticketStartTime,
                Cards = await paymentService.GetSubscriptionPaymentsForUserAsync(user.Id),
                Lines = await scheduleService.GetAllLinesAsync()
            };

            if (model.LineNumber.HasValue && model.LineType.HasValue)
            {
                model.ChosenLines = model.LineNumber.Value.ToString();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Purchase(BuySubscriptionViewModel model)
        {
            User user = await userManager.GetUserAsync(User);
            Payment payment = await paymentService.GetPaymentByNumberAsync(model.ChosenCardNumber);
            int chosenLinesCount = model.ChosenLines == null ? 0 : model.ChosenLines.Split(',').Length;

            if (chosenLinesCount == 0 || payment == null
                || payment.ExpireDate < DateTime.Now || payment.Amount < model.FinalPrice)
            {
                return Redirect(nameof(Purchase));
            }

            Purchase purchase = new();
            purchase.UserId = user.Id;

            try
            {
                PictureProcessor.GenerateReceipt(webHostEnvironment.WebRootPath, model, user, purchase.Id);
                await paymentService.PurchaseSubscriptionAsync(model, purchase);
            }
            catch (Exception e)
            {
                return Redirect(nameof(Purchase));
            }

            return Redirect("/user/account/subscriptions");
        }

        public async Task<JsonResult> GetFundsForPayment(string cardNumber)
        {
            Payment payment = await paymentService.GetPaymentByNumberAsync(cardNumber);

            if (payment == null)
            {
                return Json(false);
            }

            return Json(payment.Amount);
        }

        public async Task<JsonResult> CheckIfPaymentIsDefault(string cardNumber)
        {
            User user = await userManager.GetUserAsync(User);

            return Json(await paymentService.CheckIfPaymentIsDefaultAsync(user.Id, cardNumber));
        }

        public JsonResult GetTotalPrice(SubscriptionType subscriptionType, string lines, string duration)
        {
            decimal totalPrice = 0m;
            string cheaperOptionMsg = string.Empty;

            int chosenLinesCount = lines.Split(',').Length;

            if (subscriptionType == SubscriptionType.CARD)
            {
                if (lines.Contains("all-lines"))
                {
                    if (duration == "1-month")
                    {
                        totalPrice = Domain.CardOneMonthAll;
                    }
                    else if (duration == "3-month")
                    {
                        totalPrice = Domain.CardThreeMonthAll;
                    }
                    else if (duration == "1-year")
                    {
                        totalPrice = Domain.CardOneYearAll;
                    }
                }
                else
                {
                    if (duration == "1-month")
                    {
                        totalPrice = chosenLinesCount * Domain.CardOneMonthLine;
                        if (totalPrice > Domain.CardOneMonthAll)
                        {
                            cheaperOptionMsg = $"Карта за всички линии за месец струва {Domain.CardOneMonthAll} лв. " +
                                $"(Спестявате - {totalPrice - Domain.CardOneMonthAll} лв.!)";
                        }
                    }
                    else if (duration == "3-month")
                    {
                        totalPrice = chosenLinesCount * Domain.CardThreeMonthLine;
                        if (totalPrice > Domain.CardThreeMonthAll)
                        {
                            cheaperOptionMsg = $"Карта за всички линии за 3 месецa струва {Domain.CardThreeMonthAll} лв. " +
                                $"(Спестявате - {totalPrice - Domain.CardThreeMonthAll} лв.!)";
                        }
                    }
                    else if (duration == "1-year")
                    {
                        totalPrice = chosenLinesCount * Domain.CardOneYearLine;
                        if (totalPrice > Domain.CardOneYearAll)
                        {
                            cheaperOptionMsg = $"Карта за всички линии за 1 година струва {Domain.CardOneYearAll} лв. " +
                                $"(Спестявате - {totalPrice - Domain.CardOneYearAll} лв.!)";
                        }
                    }
                }
            }
            else if (subscriptionType == SubscriptionType.TICKET)
            {
                if (duration == "1-day")
                {
                    totalPrice = Domain.TicketOneDayAll;
                }
                else if (duration == "30-minute")
                {
                    totalPrice = Domain.TicketHalfHour;
                }
                else if (duration == "60-minute")
                {
                    totalPrice = Domain.TicketOneHour;
                }
                else if (duration == "one-way")
                {
                    totalPrice = Domain.TicketLineOneTime;
                }
            }

            return Json(new { totalPrice, cheaperOptionMsg });
        }
    }
}
