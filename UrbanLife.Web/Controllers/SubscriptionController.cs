using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using UrbanLife.Core.Utilities;
using UrbanLife.Core.Utilities.Constants;
using UrbanLife.Core.ViewModels;

namespace UrbanLife.Web.Controllers
{
    public class SubscriptionController : Controller
    {
        public IActionResult All()
        {
            return View();
        }

        public IActionResult Purchase(BuySubscriptionViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/user/login");
            }

            decimal testPrice = Domain.TicketHalfHour;

            return View(model);
        }
    }
}
