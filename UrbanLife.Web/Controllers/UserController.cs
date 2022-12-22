using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbanLife.Core.Services;
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

            if (!ModelState.IsValid || !await userService.SignUserAsync(model))
            {
                return View();
            }

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
                string fileName = await SaveImageAsync(registerViewModel.ProfilePicture);
                await userService.AddUserAsync(registerViewModel, fileName);
            }
            catch (Exception)
            {
                throw;
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

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            if (image != null && image.FileName != "guest.png")
            {
                string uniqueFileName = Guid.NewGuid()
                    .ToString()
                    .Replace('/', 'a')
                    .Replace('\\', 'b') + "==_" + image.FileName;
                
                string filePath = Path.Combine(webhostEnvironment.WebRootPath, "images");
                filePath = Path.Combine(filePath, "profile-pictures");
                filePath = Path.Combine(filePath, "custom-pictures");
                filePath = Path.Combine(filePath, uniqueFileName);

                using FileStream fileStream = new(filePath, FileMode.Create);
                await image.CopyToAsync(fileStream);

                return $"custom-pictures/{uniqueFileName}";
            }

            return "guest.png";
        }
    }
}
