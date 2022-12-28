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
        private readonly PaymentService paymentService;
        private readonly UserManager<User> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AccountController(UserService userService,
            PaymentService paymentService,
            UserManager<User> userManager,
            IWebHostEnvironment webHostEnvironment)
        {
            this.userService = userService;
            this.paymentService = paymentService;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Settings(string userId)
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

                if (model.ProfilePicture != null && user.ProfileImageName != "guest.png")
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

        public async Task<IActionResult> Payments(string userId)
        {
            if (userId == null)
            {
                return Redirect("/");
            }

            UrbanLife.Data.Data.Models.User user = await userService.GetUserByIdAsync(userId);
            List<Payment> payments = await paymentService.GetPaymentsByUser(userId);

            PaymentInfoViewModel paymentInfoViewModel = new()
            {
                AccountFirstName = user.FirstName,
                UserId = userId,
                DefaultPaymentNumber = await paymentService.GetDefaultPayment(userId),
                Payments = payments.Select(p => new PaymentMethodViewModel
                {
                    CardFirstName = p.FirstName,
                    CardLastName = p.LastName,
                    CardNumber = p.Number,
                    ExpireDate = p.ExpireDate,
                    CVC = p.CVC,
                })
                .ToList()
            };

            return View(paymentInfoViewModel);
        }

        public IActionResult AddPayment(string userId)
        {
            return View();
        }
    }
}
