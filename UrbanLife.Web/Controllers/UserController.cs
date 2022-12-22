using Microsoft.AspNetCore.Mvc;
using UrbanLife.Core.ViewModels;
#nullable disable warnings

namespace UrbanLife.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View();
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            return Redirect("/");
        }
    }
}
