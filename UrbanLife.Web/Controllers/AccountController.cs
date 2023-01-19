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

        public IActionResult Settings()
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

        public async Task<IActionResult> Payments()
        {
            UrbanLife.Data.Data.Models.User user = await userManager.GetUserAsync(User);
            List<PaymentMethodViewModel> payments = await paymentService.GetPaymentsForAccountPageAsync(user.Id);

            PaymentInfoViewModel paymentInfoViewModel = new()
            {
                AccountFirstName = user.FirstName,
                UserId = user.Id,
                Payments = payments
            };

            return View(paymentInfoViewModel);
        }

        public IActionResult AddPayment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment(BankCardViewModel model)
        {
            UrbanLife.Data.Data.Models.User user = await userManager.GetUserAsync(User);
            List<Payment> paymentsByUser = await paymentService.GetPaymentsByUserAsync(user.Id);
            Payment payment = await paymentService.GetPaymentByNumberAsync(model.CardNumber);

            if (payment != null)
            {
                if (payment.ExpireDate != model.ExpireDate ||
                payment.FirstName != model.FirstName.ToUpper() ||
                payment.LastName != model.LastName.ToUpper() ||
                payment.Amount != model.Amount ||
                payment.CVC != model.CVC)
                {
                    model.IsCardInvalid = 1;
                }
            }

            if (paymentsByUser.FirstOrDefault(p => p.Number == model.CardNumber) != null)
            {
                model.IsCardAlreadyAdded = 1;
                model.IsCardInvalid = 0;
            }

            ModelState.Clear();
            TryValidateModel(model);

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                if (payment != null)
                {
                    await paymentService.AddUserPaymentAsync(user.Id, payment.Id, model.IsDefault);
                }
                else
                {
                    await paymentService.AddPaymentMethodAsync(user.Id, model);
                }

            }
            catch (Exception)
            {
                return Redirect("/user/account/payments");
            }

            return Redirect("/user/account/payments");
        }

        public async Task<IActionResult> SetDefault(string paymentId)
        {
            UrbanLife.Data.Data.Models.User user = await userManager.GetUserAsync(User);

            await paymentService.SetDefaultPaymentAsync(user.Id, paymentId);

            return Redirect("/user/account/payments");
        }

        public async Task<IActionResult> Delete(string paymentId)
        {
            UrbanLife.Data.Data.Models.User user = await userManager.GetUserAsync(User);

            try
            {
                await paymentService.DeletePaymentAsync(user.Id, paymentId);
            }
            catch (InvalidOperationException)
            {
                return Redirect("/user/account/payments");
            }

            return Redirect("/user/account/payments");
        }

        public IActionResult Subscriptions()
        {
            return View();
        }
    }
}
