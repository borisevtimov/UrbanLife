using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbanLife.Core.Services;
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

        //[Route("/user/account/settings/{id}")]
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

            string? fileName = null;

            if (model.ProfilePicture != null)
            {
                fileName = await SaveImageAsync(model.ProfilePicture);
                DeletePicture(user);
            }

            await userService.UpdateProfileAsync(user, model, fileName);

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

                string filePath = Path.Combine(webHostEnvironment.WebRootPath, "images");
                filePath = Path.Combine(filePath, "profile-pictures");
                filePath = Path.Combine(filePath, "custom-pictures");
                filePath = Path.Combine(filePath, uniqueFileName);

                using FileStream fileStream = new(filePath, FileMode.Create);
                await image.CopyToAsync(fileStream);

                return $"custom-pictures/{uniqueFileName}";
            }

            return "guest.png";
        }

        private void DeletePicture(User user)
        {
            string customPicturesPath = Path.Combine(webHostEnvironment.WebRootPath, "images");
            customPicturesPath = Path.Combine(customPicturesPath, "profile-pictures");
            customPicturesPath = Path.Combine(customPicturesPath, user.ProfileImageName);

            if (System.IO.File.Exists(customPicturesPath))
            {
                System.IO.File.Delete(customPicturesPath);
            }
        }
    }
}
