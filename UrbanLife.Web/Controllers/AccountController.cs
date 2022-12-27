using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbanLife.Core.Services;
using UrbanLife.Core.Utilities;
using UrbanLife.Core.ViewModels;
using UrbanLife.Data.Data.Models;

namespace UrbanLife.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserService userService;
        private readonly UserManager<User> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AccountController(UserService userService,
            UserManager<User> userManager,
            IWebHostEnvironment webHostEnvironment)
        {
            this.userService = userService;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Settings(string id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Settings(UpdateProfileViewModel model)
        {
            if (model.Email != null)
            {
                if (await userService.UserExistsAsync(model.Email))
                {
                    model.EmailAlreadyUsed = 1;
                }
            }

            User user = await userManager.GetUserAsync(User);

            if (model.Password != null && model.ConfirmPassword != null)
            {
                if (await userService.PasswordExists(user.Email, model.Password))
                {
                    model.SamePassword = 1;
                }
            }

            ModelState.Clear();
            TryValidateModel(model);

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                string? fileName = null;

                if (model.ProfilePicture != null)
                {
                    fileName = await PictureProcessor
                        .DownloadProfilePictureAsync(webHostEnvironment.WebRootPath, model.ProfilePicture);

                    PictureProcessor.DeleteProfilePicture(webHostEnvironment.WebRootPath, user.ProfileImageName);
                }

                await userService.UpdateProfileAsync(user, model, fileName);
            }
            catch (IOException)
            {
                return View();
            }

            return Redirect("/");
        }
    }
}
