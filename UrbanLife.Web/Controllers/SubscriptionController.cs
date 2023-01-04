using Microsoft.AspNetCore.Mvc;

namespace UrbanLife.Web.Controllers
{
    public class SubscriptionController : Controller
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
