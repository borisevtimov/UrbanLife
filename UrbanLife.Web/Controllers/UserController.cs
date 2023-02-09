using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrbanLife.Core.Services;
using UrbanLife.Core.Utilities;
using UrbanLife.Core.ViewModels;
using UrbanLife.Data.Data.Models;
#nullable disable warnings

namespace UrbanLife.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService userService;
        private readonly SignInManager<User> signInManager;
        private readonly IWebHostEnvironment webhostEnvironment;

        public UserController(UserService userService,
            SignInManager<User> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            this.userService = userService;
            this.signInManager = signInManager;
            this.webhostEnvironment = webHostEnvironment;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            await userService.SignUserAsync(model);

            return Redirect("/");
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
        public async Task<IActionResult> RegisterAsync(RegisterViewModel registerViewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (await userService.UserExistsAsync(registerViewModel.Email))
            {
                return View();
            }

            try
            {
                string fileName = await PictureProcessor
                    .DownloadProfilePictureAsync(webhostEnvironment.WebRootPath, registerViewModel.ProfilePicture);

                await userService.AddUserAsync(registerViewModel, fileName);
            }
            catch (IOException)
            {
                return View();
            }

            return Redirect("/");
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await signInManager.SignOutAsync();
            }

            return Redirect("/");
        }

        public async Task<JsonResult> CheckLoginCredentials(string email, string password)
        {
            if (email == null || password == null)
            {
                return Json(false);
            }

            return Json(await userService.PasswordExistsAsync(email, password));
        }

        public async Task<JsonResult> IsEmailAddressUsed(string email)
        {
            if (email == null)
            {
                return Json(false);
            }

            return Json(await userService.UserExistsAsync(email));
        }
    }
}
