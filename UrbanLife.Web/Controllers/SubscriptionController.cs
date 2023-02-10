using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbanLife.Core.Services;
using UrbanLife.Core.Utilities.Constants;
using UrbanLife.Core.ViewModels;
using UrbanLife.Data.Data.Models;

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

        public async Task<IActionResult> Purchase(BuySubscriptionViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/user/login");
            }

            UrbanLife.Data.Data.Models.User user = await userManager.GetUserAsync(User);
            model.Cards = await paymentService.GetSubscriptionPaymentsForUserAsync(user.Id);
            model.Lines = await scheduleService.GetAllLinesAsync();

            if (model.LineNumber.HasValue && model.LineType.HasValue)
            {
                model.ChosenLines = model.LineNumber.Value.ToString();
            }

            //string previousPage = Request.Headers["Referer"];
            //
            //if (previousPage.ToLower().Contains("/schedule/line"))
            //{
            //
            //}
            //
            //decimal testPrice = Domain.TicketHalfHour;

            return View(model);
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
    }
}
