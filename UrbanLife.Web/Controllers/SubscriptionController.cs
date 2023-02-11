using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbanLife.Core.Services;
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

        public SubscriptionController(PaymentService paymentService,
             UserManager<User> userManager,
             ScheduleService scheduleService)
        {
            this.paymentService = paymentService;
            this.userManager = userManager;
            this.scheduleService = scheduleService;
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
                TicketStartTime = ticketStartTime,
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
        public IActionResult Purchase(BuySubscriptionViewModel model)
        {
            return RedirectToAction(nameof(Purchase));
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

        public JsonResult GetTotalPrice(SubscriptionType subscriptionType, string lines, string duration)
        {
            decimal totalPrice = 0m;
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
                    }
                    else if (duration == "3-month")
                    {
                        totalPrice = chosenLinesCount * Domain.CardThreeMonthLine;
                    }
                    else if (duration == "1-year")
                    {
                        totalPrice = chosenLinesCount * Domain.CardOneYearLine;
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

            return Json(totalPrice);
        }
    }
}
