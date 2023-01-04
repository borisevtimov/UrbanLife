using Microsoft.AspNetCore.Mvc;

namespace UrbanLife.Web.Controllers
{
    public class TravelController : Controller
    {
        public IActionResult Choose()
        {
            return View();
        }
    }
}
