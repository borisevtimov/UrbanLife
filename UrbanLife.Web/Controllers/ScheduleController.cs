using Microsoft.AspNetCore.Mvc;

namespace UrbanLife.Web.Controllers
{
    public class ScheduleController : Controller
    {
        public IActionResult Lines()
        {
            return View();
        }
    }
}
